using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityOSC;
using Z;
public class OSCWordReader
{
    public int readIndex = 0;
    public int segmentIndex = 0;
    public int subIndex = 0;
    const int wordDivision = 4;

    public bool isAligned { get { return subIndex == 0; } }

    public void Advance(int steps, byte[] data)
    {
        for (int i = 0; i < steps; i++)
        {
            Advance(data);
        }
    }
    public void Advance(byte[] data)
    {
        readIndex++;
        subIndex++;
        if (subIndex >= wordDivision)
        {
            subIndex = 0;
            segmentIndex++;
        }
        string s = "";
        for (int i = 0; i < readIndex; i++)
        {
            s += data[i] == 0 ? "-" : (char)data[i] + " ";
        }

        Debug.Log(((s + "                    " + this + " ->").MakeColor(new Color(0.3f, 0.6f, 0.1f))));
    }
    public void Align(byte[] data)
    {
        Debug.Log(((" aligning start " + this + "Align").MakeColor(new Color(0.6f, 0.4f, 0.8f))).Small());
        //	Advance(data);

        while (readIndex < data.Length && data[readIndex - 1] != 0)
            Advance(data);
        while (subIndex != 0) Advance(data);
    }
    public override string ToString()
    {
        return "r=" + readIndex + " seg=" + segmentIndex + " sub=" + subIndex;
    }
}

public static class zOSCExtentions
{

    static byte[] buff4 = new byte[4]; //lock them !!
    static byte[] buff8 = new byte[8]; // lock them
    public static string OSCRemoveLeadingSlash(this string address)
    {
        if (address.Length > 1)
            if (address[0] == '/') return address.Substring(1);
        return address;
    }

    public static string OSCEnsureLeadingSlash(this string address)
    {
        if (address.Length > 1)
            if (address[0] != '/') return '/' + address;
        return address;
    }
    public static string ToReadableString(this OSCPacket message)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.Append(message.Address);
        sb.Append(" ");
        //sb.Append(message.typeTag);
      //  sb.Append(" ");
        for (int i = 0; i < message.typeTag.Length - 1; i++)
        {
            char thistype = message.typeTag[i + 1];
            sb.Append(" " + thistype + " :");
            switch (thistype)
            {
                case 'f':
                    var msgf = message.GetFloat(i);
                    sb.Append("[" + msgf.ToString("F") + "]");
                    break;
                case 's':
                    var msgs = message.GetString(i);
                    sb.Append("[" + msgs + "]");
                    break;
                case 'i':
                    var msgi = message.GetInt(i);
                    sb.Append("[" + msgi + "]");
                    break;
                case 'h':
                    var msgh = message.GetULong(i);
                    sb.Append("[" + msgh + "]");
                    break;
                case 'u':
                    var msgu = message.GetULong(i);
                    sb.Append("[" + msgu.ToFingerprintString() + "]");
                    break;
                case 'b':
                    var msgb = message.GetBytes(i);
                    sb.Append("[" + msgb.ByteArrayToStringAsHex(0, 30) + "]");
                    break;
            }
        }
        return sb.ToString();
    }

    public static void Append<T>(this zOSCMessage message, T objectToAdd)
    {
        if (message.objects == null || message.types == null)
        {
            message.objects = new List<object>();

        }
        message.objects.Add(objectToAdd);
        message.types.Add(objectToAdd.GetType());
    }

    public static char GetTypeTagChar<T>(T val)
    {
        if (val is System.Single) return 'f';
        if (val is System.String) return 's';
        if (val is byte[]) return 'b';
        if (val is System.Int32) return 'i';
        if (val is System.Int64) return 'h';
        if (val is System.Double) return 'd';
        if (val is System.UInt64) return 'u';
        return ','; ;
    }

    // static byte[] buff4 = new byte[4];
    // static byte[] buff8 = new byte[8];

    public static int UnpackInt(byte[] data, ref int start)
    {
        Array.Copy(data, start, buff4, 0, 4);
        start += 4;

        if (BitConverter.IsLittleEndian)
            buff4 = SwapEndian(buff4);
        return BitConverter.ToInt32(buff4, 0);
    }
    public static float UnpackFloat(byte[] data, ref int start)
    {
        Array.Copy(data, start, buff4, 0, 4);
        start += 4;

        if (BitConverter.IsLittleEndian)
            buff4 = SwapEndian(buff4);
        return BitConverter.ToSingle(buff4, 0);
    }

    public static double UnpackDouble(byte[] data, ref int start)
    {
        Array.Copy(data, start, buff8, 0, 8);
        start += 8;

        if (BitConverter.IsLittleEndian)
            buff8 = SwapEndian(buff8);
        return BitConverter.ToDouble(buff8, 0);
    }

    public static long UnpackLong(byte[] data, ref int start)
    {
        Array.Copy(data, start, buff8, 0, 8);
        start += 8;
        if (BitConverter.IsLittleEndian)
            buff8 = SwapEndian(buff8);
        return BitConverter.ToInt64(buff8, 0);
    }

    //ystem.Text.Encoding.Default.GetString(result);
    // 	byte[] buffer = System.Text.Encoding.UTF8.GetBytes(convert);

    // // From byte array to string
    // string s = System.Text.Encoding.UTF8.GetString(buffer, 0, buffer.Length);
    public static int AdvanceOscSubIndex(this int subIndex, ref int startOscSubFrame)
    {
        subIndex++;
        while (subIndex >= 3)
        {
            subIndex -= 4;
            startOscSubFrame++;
        }
        return subIndex;

    }
    public static void AddTypeFromChar(this List<System.Type> types, char c)
    {
        switch (c)
        {
            case 'f':
                types.Add(typeof(System.Single));
                break;
            case 's':
                types.Add(typeof(System.String));
                break;
            case 'b':
                types.Add(typeof(byte[]));
                break;
            case 'i':
                types.Add(typeof(System.Int32));
                break;
            case 'h':
                types.Add(typeof(System.Int64));
                break;
            case 'u':
                types.Add(typeof(System.UInt64));
                break;
            case 'd':
                types.Add(typeof(System.Double));
                break;
        }
    }
    public static Type GetTypeFromChar(this char c)
    {
        switch (c)
        {
            case 'f':
                return typeof(System.Single);
            case 's':
                return typeof(System.String);
            case 'b':
                return typeof(byte[]);
            case 'i':
                return typeof(System.Int32);
            case 'h':
                return typeof(System.Int64);
            case 'u':
                return typeof(System.UInt64);
            case 'd':
                return typeof(System.Double);
        }
        return null;
    }
    public static List<System.Type> UnpackTypeTag(this zOSCMessage message, OSCWordReader reader)
    {
        UnityEngine.Debug.Log("Starting ubpack of typetags");
        byte[] data = message.data;
        string typetagstring = data.UnpackString(reader);
        var list = new List<System.Type>();
        while (data[reader.readIndex] != 0)
        {
            list.AddTypeFromChar((char)data[reader.readIndex]);
            reader.Advance(data);
        }
        return list;
    }
    public static int CountNonZeos(this byte[] bytes, int readindex = 0)
    {
        int count = 0;

        while (readindex < bytes.Length && bytes[readindex] != 0)
        {
            count++;
            readindex++;

            // Debug.Log("counting nonzeros " + readindex + " c " + count + " lastbyte " + ((char) bytes[readindex - 1]));
        }
        // Debug.Log("counting returinig " + count);
        return count;
    }

    // public static string UnpackString(this byte[] data, ref int startOscSubFrame)
    // {
    // 	int mult4 = startOscSubFrame * 4;
    // 	var s = Encoding.ASCII.GetString(data, mult4, data.CountNonZeos(mult4));
    // 	startOscSubFrame += (s.Length / 4 + 1);
    // 	return s;
    // }
    public static string UnpackString(this byte[] data, OSCWordReader reader)
    {
        //	int mult4 = startOscSubFrame * 4;
        Debug.Log("counting zeroes from " + reader.readIndex + " = " + data.CountNonZeos(reader.readIndex));
        var s = Encoding.ASCII.GetString(data, reader.readIndex, data.CountNonZeos(reader.readIndex));
        Debug.Log(" unpacked string " + s.Length + " " + s);
        reader.Advance(s.Length, data);
        reader.Align(data);
        return s;
    }
    public static byte[] UnpackBytes(byte[] data, ref int start)
    {
        int length = UnpackInt(data, ref start);
        byte[] buffer = new byte[length];
        Array.Copy(data, start, buffer, 0, buffer.Length);
        start += buffer.Length;
        start = ((start + 3) / 4) * 4;
        return buffer;
    }

    // public static zOSCMessage Unpack(byte[] data)
    // {
    // 	int start = 0;
    // 	return Unpack(data, ref start, data.Length);
    // }

    // public static zOSCMessage Unpack(byte[] data, ref int start, int end)
    // {
    // 	if (data[start] == '#')
    // 	{
    // 		//   return OSCBundle.Unpack(data, ref start, end);
    // 	}

    // 	// else 
    // 	return zOSCMessage.Unpack(data, ref start);
    // }

    public static void PadNull(List<byte> data)
    {
        byte nullvalue = 0;
        int pad = 4 - (data.Count % 4);
        for (int i = 0; i < pad; i++)
            data.Add(nullvalue);
    }

    public static byte[] PackValue<T>(T value)
    {
        object valueObject = value;

        //   Type type = value.GetType();
        byte[] data = null;

        if (value is float)
        {
            data = BitConverter.GetBytes((float)valueObject);
            if (BitConverter.IsLittleEndian) data = SwapEndian(data);
            return data;
        }
        // break;

        if (value is String)

            return Encoding.ASCII.GetBytes((string)valueObject);

        if (value is byte[])
        {
            byte[] valueData = ((byte[])valueObject);
            List<byte> bytes = new List<byte>();
            bytes.AddRange(PackValue(valueData.Length));
            bytes.AddRange(valueData);
            data = bytes.ToArray();
            return data;
        }
        if (value is System.Int32)
        {
            data = BitConverter.GetBytes((int)valueObject);
            if (BitConverter.IsLittleEndian) data = SwapEndian(data);
            // UnityEngine.Debug.Log("data lent "+data.Length);
            if (data.Length < 4)
            {
                UnityEngine.Debug.Log("int too short");
                List<byte> blist = new List<byte>(data);
                while (blist.Count < 4)
                {
                    blist.Add(0);
                    UnityEngine.Debug.Log("adding 0");
                }
                data = blist.ToArray();
            }
            return data;
        }
        if (value is System.Int64)
        {
            data = BitConverter.GetBytes((long)valueObject);
            if (BitConverter.IsLittleEndian) data = SwapEndian(data);
            return data;
        }
        else
            throw new Exception("Unsupported data type.");
        //    return data;
    }

    public static byte[] SwapEndian(byte[] data)
    {
        byte[] swapped = new byte[data.Length];
        for (int i = data.Length - 1, j = 0; i >= 0; i--, j++)
        {
            swapped[j] = data[i];
        }
        return swapped;
    }

}
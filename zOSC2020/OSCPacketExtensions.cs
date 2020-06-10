///zambari codes unity

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;
using UnityOSC;
using Z;

// extensions of UnityOSC's packet class by zambari/stereoko.tv 2017
// v.0.1
// V.0.2 postmerge
public static class OSCPacketExtensions
{

    // public class PositionAndRotation
    // {
    //     public Vector3 position;
    //     public Quaternion rotation;
    //     public PositionAndRotation()
    //     {

    //     }
    //     public PositionAndRotation(Vector3 position, Quaternion rotation)
    //     {
    //         this.position = position;
    //         this.rotation = rotation;
    //     }

    // }

    public static string SanitizeOSCAddress(this string source)
    {
        if (String.IsNullOrEmpty(source)) return "/none";
        string newAddress = "";

        if (source[0] != '/') newAddress = "/";
        for (int i = 0; i < source.Length; i++)
        {
            char thisChar = source[i];
            if (thisChar == '/') newAddress += thisChar;
            if (Char.IsLetterOrDigit(thisChar)) newAddress += thisChar;

        }
        if (newAddress.Length == 1) newAddress += "none";
        return newAddress;

    }
    public static float GetFloat(this OSCPacket packet, int index)
    {
        packet.AssertTypeTag(index, 'f');
        return Convert.ToSingle(packet.Data[index]);

    }
    public static string GetString(this OSCPacket packet, int index = 0)
    {
        packet.AssertTypeTag(index, 's');
        return packet.Data[index].ToString(); //System.Convert.ToString(packet.Data[index]);

    }
    public static string GetString(this OSCPacket packet, ref int index)
    {
        index++;
        return packet.GetString(index - 1);
    }
    public static byte[] GetBytes(this OSCPacket packet, int index = 0)
    {
        // .ToString();
        //   packet.AssertTypeTag(index, 'b');

        return (byte[]) packet.Data[index];
    }

    public static byte[] GetBytesSOMESTRANGEVERSION(this OSCPacket packet, int index = 0)
    {

        //value = packet.Data[index] as byte[];
        //if (value != null)
        //   return true;
        packet.AssertTypeTag(index, 'b');
        Debug.Log("iubdex " + index); //+" data  "
        byte[] bytes = packet.Data[index].ObjectToByteArray();
        int len = BitConverter.ToInt32(bytes, 0);

        // for(int i=0;i<packet.Data.Count;i++)

        // {
        //     UnityEngine.Debug.Log(i+" --- "+packet.Data[i].GetType().ToString());
        // }
        // if (packet.typeTag[index + 1] != 'b')
        // {
        //     value = new byte[0];
        //     return false;
        // }
        // byte[] bytes = packet.Data[index].ObjectToByteArray();
        string dump = "di";
        for (int i = 0; i < bytes.Length; i++)
        {
            dump += "val " + i + " = " + bytes[i];
        }
        Debug.Log(dump);
        Debug.Log("copying values  bledn " + len + " bytes len " + bytes.ByteArrayToStringAsHex());
        var value = new byte[len];
        System.Array.Copy(bytes, 4, value, 0, len);
        //  if (Int32.TryParse(packet.Data[0].ToString(), out len))
        //   value = packet.Data[0].ToString();
        return value;
    }

    public static void SwapEndianFourBytes(this byte[] source)
    {
        Debug.Log("swatping");
        byte a = source[0];
        byte b = source[1];
        byte c = source[2];
        byte d = source[3];
        source[0] = d;
        source[1] = c;
        source[2] = b;
        source[3] = a;
    }
    public static byte[] ObjectToByteArray(this System.Object obj)
    {
        if (obj == null)
            return null;
        System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
        using(System.IO.MemoryStream ms = new System.IO.MemoryStream())
        {
            bf.Serialize(ms, obj);
            return ms.ToArray();
        }
    }
    public static bool GetStringArray(this OSCPacket packet, out string[] value)
    {
        value = new string[0];
        if (packet.typeTag.Length < 3) return false;
        for (int i = 1; i < packet.typeTag.Length; i++)
            if (packet.typeTag[i] != 's') return false;
        value = new string[packet.typeTag.Length - 1];
        for (int i = 0; i < packet.typeTag.Length - 1; i++)
            value[i] = packet.Data[i].ToString();
        return true;
    }
    public static float GetFloat(this OSCPacket packet, out float value, int index = 0)
    {
        value = 0;
        if (packet.typeTag[index + 1] != 'f')
            Debug.Log("Warning, osc typetagmismatch");
        return System.Convert.ToSingle(packet.Data[index]);

    }

    public static float GetFloat(this OSCPacket packet)
    {
        return packet.GetFloat(packet.currentRreadIndex++);
    }

    public static int GetInt(this OSCPacket packet)
    {
        packet.AssertTypeTag(packet.currentRreadIndex, 'i');
        return System.Convert.ToInt32(packet.Data[packet.currentRreadIndex++]);
    }

    public static int GetInt(this OSCPacket packet, int index = 0)
    {
        packet.AssertTypeTag(index, 'i');
        return System.Convert.ToInt32(packet.Data[index]);
    }

    public static int GetInt(this OSCPacket packet, ref int index)
    {
        index++;
        return packet.GetInt(index - 1);
    }
    public static float GetFloat(this OSCPacket packet, ref int index)
    {
        index++;
        return packet.GetFloat(index - 1);
    }
    // public static string AsString(this OSCPacket packet)
    // {
    //     string typeTag = packet.typeTag;
    //     StringBuilder sb = new StringBuilder();
    //     sb.Append("<b>" + packet.Address + "</b>");
    //     if (typeTag.Length > 1)
    //     {
    //         sb.Append(" [");
    //         sb.Append(typeTag);
    //         sb.Append("] ");
    //         for (int i = 0; i < typeTag.Length - 1; i++)
    //         {

    //             switch (typeTag[i + 1])
    //             {
    //                 case 's':
    //                     sb.Append(packet.Data[i].ToString());
    //                     break;
    //                 case 'f':
    //                     float f = -99;
    //                     if (Single.TryParse(packet.Data[i].ToString(), out f))
    //                         sb.Append((Mathf.Round(f * 100) / 100).ToString());
    //                     else sb.Append("[X]");
    //                     break;
    //                 case 'i':
    //                     int ii;
    //                     if (Int32.TryParse(packet.Data[i].ToString(), out ii))
    //                         sb.Append(ii.ToString());
    //                     else sb.Append("[X]");

    //                     break;
    //                 case 'b':
    //                     sb.Append("[BLOB]");

    //                     break;

    //             }
    //             sb.Append(" ");
    //         }
    //     }
    //     //        Debug.Log(sb.ToString());
    //     return sb.ToString();

    // }
    // public static bool GetQuaternion(this OSCPacket packet, out Quaternion q)
    // {
    //     q = Quaternion.identity;
    //     if (packet.typeTag[1] != 'f' ||
    //         packet.typeTag[2] != 'f' ||
    //         packet.typeTag[3] != 'f' ||
    //         packet.typeTag[4] != 'f') return false;
    //     float f1, f2, f3, f4;
    //     if (Single.TryParse(packet.Data[0].ToString(), out f1))
    //         if (Single.TryParse(packet.Data[1].ToString(), out f2))
    //             if (Single.TryParse(packet.Data[2].ToString(), out f3))
    //                 if (Single.TryParse(packet.Data[3].ToString(), out f4))
    //                 {
    //                     q = new Quaternion(f1, f2, f3, f4);
    //                     return true;
    //                 }

    //     return false;
    // }

    // public static bool GetPositionAndRotation(this OSCPacket packet, out PositionAndRotation pr)
    // {
    //     pr = new PositionAndRotation();
    //     if (packet.typeTag[1] != 'f' ||
    //         packet.typeTag[2] != 'f' ||
    //         packet.typeTag[3] != 'f' ||
    //         packet.typeTag[4] != 'f' ||
    //         packet.typeTag[5] != 'f' ||
    //         packet.typeTag[6] != 'f' ||
    //         packet.typeTag[7] != 'f') return false;

    //     float f1, f2, f3, f4, f5, f6, f7;
    //     if (Single.TryParse(packet.Data[0].ToString(), out f1))
    //         if (Single.TryParse(packet.Data[1].ToString(), out f2))
    //             if (Single.TryParse(packet.Data[2].ToString(), out f3))
    //                 if (Single.TryParse(packet.Data[3].ToString(), out f4))
    //                     if (Single.TryParse(packet.Data[4].ToString(), out f5))
    //                         if (Single.TryParse(packet.Data[5].ToString(), out f6))
    //                             if (Single.TryParse(packet.Data[6].ToString(), out f7))
    //                             {
    //                                 Vector3 pos = new Vector3(f1, f2, f3);
    //                                 Quaternion rot = new Quaternion(f4, f5, f6, f7);
    //                                 pr = new PositionAndRotation(pos, rot);
    //                                 return true;
    //                             }

    //     return false;
    // }

    // public static bool GetVector2(this OSCPacket packet, out Vector2 v)
    // {
    //     v = Vector2.zero;
    //     float f1, f2;
    //     if (packet.typeTag[1] != 'f' ||
    //         packet.typeTag[2] != 'f'
    //     ) return false;
    //     if (Single.TryParse(packet.Data[0].ToString(), out f1))
    //         if (Single.TryParse(packet.Data[1].ToString(), out f2))
    //         {
    //             v = new Vector2(f1, f2);
    //             return true;
    //         }

    //     return false;
    // }
    // public static bool GetVector3(this OSCPacket packet, out Vector3 v)
    // {
    //     v = Vector3.zero;
    //     float f1, f2, f3;
    //     if (packet.typeTag[1] != 'f' ||
    //         packet.typeTag[2] != 'f' ||
    //         packet.typeTag[3] != 'f') return false;
    //     if (Single.TryParse(packet.Data[0].ToString(), out f1))
    //         if (Single.TryParse(packet.Data[1].ToString(), out f2))
    //             if (Single.TryParse(packet.Data[2].ToString(), out f3))
    //             {
    //                 v = new Vector3(f1, f2, f2);
    //                 return true;
    //             }

    //     return false;
    // }
    // public static bool getfloatArray(this OSCPacket packet, out float[] v)
    // {
    //     v = new float[4];

    //     if (packet.typeTag[1] != 'f' ||
    //         packet.typeTag[2] != 'f' ||
    //         packet.typeTag[3] != 'f' ||
    //         packet.typeTag[4] != 'f') return false;
    //     if (Single.TryParse(packet.Data[0].ToString(), out v[0]))
    //         if (Single.TryParse(packet.Data[1].ToString(), out v[1]))
    //             if (Single.TryParse(packet.Data[2].ToString(), out v[2]))
    //                 if (Single.TryParse(packet.Data[2].ToString(), out v[3]))
    //                     return true;

    //     return false;
    // }

    public static byte[] Serialize(this List<object> list)
    {

        byte[] bytes;
        MemoryStream ms = new MemoryStream();

        new BinaryFormatter().Serialize(ms, list);

        bytes = ms.ToArray();
        ms.Flush();
        ms.Dispose();

        return bytes;

    }
    public static List<object> Deserialize(this byte[] bytes)
    {
        List<object> list = new List<object>();
        Debug.Log("deserialized len " + bytes.Length);
        using(var ms = new MemoryStream(bytes, 0, bytes.Length))
        {
            ms.Write(bytes, 0, bytes.Length);
            ms.Position = 0;
            try
            {
                var data = new BinaryFormatter().Deserialize(ms);
                list = (List<object>) data;
            }
            catch (Exception e)
            {
                Debug.Log("deserializing exception " + e.Message);
            }
            Debug.Log("done");
        }
        return list;
    }
    // public static byte[] ObjectToByteArray(this object obj)
    // {
    //     if (obj == null)
    //         return null;
    //     BinaryFormatter bf = new BinaryFormatter();
    //     using (MemoryStream ms = new MemoryStream())
    //     {
    //         bf.Serialize(ms, obj);
    //         byte[] b = ms.ToArray();
    //         byte[] c = b.Slice(27, b.Length - 1);
    //         Debug.Log("serialized len " + c.Length);
    //         return c;
    //     }
    // }

    // static bool checkIfStartedAndStringOk(string s)
    // {
    //     if (String.IsNullOrEmpty(s))
    //     {
    //         return false;
    //     }

    //     if (s[0] != '/' || (s.Length < 2))
    //     {
    //         Debug.Log("osc adresses should start with '/'");
    //         return false;
    //     }
    //     if (_instance == null)
    //     {
    //         Debug.Log("zOSC_1 not started !");
    //         return false;
    //     }
    //     return true;
    // }   

    /// <summary>
    /// Get the array slice between the two indexes.
    /// ... Inclusive for start index, exclusive for end index.
    /// </summary>
    public static T[] Slice<T>(this T[] source, int start, int end)
    {
        // Handles negative ends.
        if (end < 0)
        {
            end = source.Length + end;
        }
        int len = end - start;

        // Return new array.
        T[] res = new T[len];
        for (int i = 0; i < len; i++)
        {
            res[i] = source[i + start];
        }
        return res;
    }
}

// and a snippet from the stack overflow
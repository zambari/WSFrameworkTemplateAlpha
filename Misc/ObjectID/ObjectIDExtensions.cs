using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

// v.02 more finds
// v.03 strings

public interface IProvideLabel
{

    GameObject gameObject { get; }
    Transform transform { get; }
    string label { get; }

}
public static class ObjectIDExtensions
{

    public static ulong GetID(this GameObject g)
    {
        var id = g.GetComponent<ObjectID>();
        if (id != null) return id.identifier;
        else return 0;
    }

    public static ulong GetID(this Transform g)
    {
        var id = g.GetComponent<ObjectID>();
        if (id != null) return id.identifier;
        else return 0;
    }
    public static GameObject FindObject(this ulong val)
    {
        if (ObjectID.objectDict == null) return null;
        GameObject obj;
        ObjectID.objectDict.TryGetValue(val, out obj);
        return obj;
    }
    public static Transform FindTransform(this ulong val)
    {
        if (ObjectID.objectDict == null) return null;
        GameObject obj;
        ObjectID.objectDict.TryGetValue(val, out obj);
        if (obj == null) return null;
        return obj.transform;
    }
    public static ulong GetObjectID(this Component source, bool createIfNotFound = false)
    {
        if (source == null){

                return 0;
        } 
        if (createIfNotFound)
        return source.gameObject.AddOrGetComponent<ObjectID>().identifier;

        return GetObjectID(source.transform);
    }
    static System.Random _random;
    public static System.Random random
    {
        get
        {
            if (_random == null)
            {
                Debug.Log("  rnd = null");
                _random = new System.Random();
            }
            return _random;
        }
    }
    public static ulong Compact(this ulong id)
    {
        return ((ulong)((Int32)(id ^ (id >> 32))) << 32);
    }

    public static ulong MergeWithInt(this ulong id, int value) //nextValueIndex
    {
        return id.Compact() | (ulong)(long)value;
    }
    static byte[] bytes = new byte[8];
    public static ulong ObtainRandomme()
    {
        random.NextBytes(bytes);
        return BitConverter.ToUInt64(bytes, 0);
    }
    public static ulong GetWhatevID(GameObject source)
    {
#if UNITY_EDITOR
        //ulong newidentifier = ObjectIDExtensions.CreateNewTimeAndInstanceBasedIdentifierFromObject(source); // in editopr we take time and instance id
        ulong newidentifier = ObjectIDExtensions.CreateNewTimeAndInstanceBasedIdentifier(0); // in editopr we take time and instance id
#else
        ulong newidentifier = ObjectIDExtensions.CreateHierarchyBasedIdentified(source, 0);
#endif
        return newidentifier>>2; //wyt
    }
    public static bool CreateAndValidateIdentifier(ObjectID obj)
    {
        //   if (obj==null) return false;
        ulong newidentifier = 0;
        for (int i = 0; i < 5; i++)
        {
            newidentifier = GetWhatevID(obj.gameObject);
            if (newidentifier != 0)
            {
                if (ObjectID.RegisterID(newidentifier, obj))
                    return true;
                else
                {
                    var nt = newidentifier.FindTransform();
                    Debug.Log(obj.name + " failed registering " + newidentifier.ToFingerprintString() + " occupied by " + (nt == null ? " no one apparently " : nt.name), obj.gameObject);
                }
            } /// outside edito we try to use hierarchys
            // id = id & mask;
            //  Debug.Log(" for " + nameChain + " the hash is " + id.ToStringAsHex());
            //     
        }
        //  Debug.Log("Failed getting unieqe iod ");
        return false;
    }
    public static ulong CreateHierarchyBasedIdentified(GameObject g, int randomLevel)
    {
        Transform t = g.transform;
        string nameChain = t.name + t.GetSiblingIndex().ToString();
        while (t.parent != null)
        {
            t = t.parent;
            nameChain = t.name + "/" + nameChain + randomLevel * 29; //wutmate
        }
        ulong id = GetHashFromString(nameChain);
        return id;
    }
    public static ulong PackInUpper(this ulong source)
    {
        var bytes = BitConverter.GetBytes(source);
        byte a = (byte)(bytes[7] + bytes[0]);
        byte b = (byte)(bytes[6] + bytes[1]);
        byte c = (byte)(bytes[5] + bytes[2]);
        byte d = (byte)(bytes[4] + bytes[3]);

        bytes[7] = d;
        bytes[6] = c;
        bytes[5] = b;
        bytes[4] = a;
        bytes[3] = 0;
        bytes[2] = 0;
        bytes[1] = 0;
        bytes[0] = 0;
        ulong vOut = BitConverter.ToUInt64(bytes, 0);
        return vOut; // Convert.ToUInt64(bytes);

    }
    public static ulong CreateNewTimeAndInstanceBasedIdentifier(Int32 instanceid)
    {
        ulong newId = ((ulong)(System.DateTime.Now.Ticks >> 16 & (System.Int32.MaxValue >> 1))) << 32; //^ g.GetInstanceID()
        // return
        //      newId=newId|(UInt64)(long)(yetanothercounter<<56);
        ulong truncatedInstancid = (ulong)instanceid; // & 0b111111111111111;
        //incrementedOnIDGeneration += 2;

        truncatedInstancid = (truncatedInstancid >> 16) ^ truncatedInstancid;
        //   truncatedInstancid = truncatedInstancid ^( (truncatedInstancid >> 3)^(truncatedInstancid << 3) ) ;//^ (truncatedInstancid << 3);
        //   if ((yetanothercounter) % 6 == 0)
        //   truncatedInstancid=truncatedInstancid^(truncatedInstancid<<16);
        //  if ((yetanothercounter+1) % 6 == 0)
        //           truncatedInstancid=truncatedInstancid|(truncatedInstancid<<16);
        //      if ((yetanothercounter+2) % 6 == 0)
        //           truncatedInstancid=truncatedInstancid&(truncatedInstancid<<16);
        truncatedInstancid = (truncatedInstancid >> 16) << 16;

        //   truncatedInstancid=((truncatedInstancid>>16 )^ ((truncatedInstancid ) )^0xffff);
        //    truncatedInstancid = truncatedInstancid | (truncatedInstancid >> 4);
        //truncatedInstancid=(truncatedInstancid>>16^  truncatedInstancid )<<16;

        // truncatedInstancid=truncatedInstancid;
        newId = newId | (UInt32)(truncatedInstancid | (ulong)(long)ObjectID.incremental);
        // newId = 0 |( (ulong) ((yetanothercounter) << 16)) |(ulong)(long)( truncatedInstancid);

        // UInt32 x = (UInt32) (truncatedInstancid + (int) yetanothercounter);
        //   Int64 y = (Int64) (truncatedInstancid + (int) yetanothercounter);
        //   Debug.Log(newId.ToFingerprintString() + " creatbg   x  " + ((ulong) (x)).ToFingerprintString() + "     " + "  y  " + ((ulong) (y)).ToStringAsHex() + "     ");
        //           Debug.Log(" tick " + newId.ToFingerprintString() + "   x  " + newId.ToStringAsHex() + "     " + newId);

        return newId.PackInUpper();
    }
    // public static ulong CreateNewTimeAndInstanceBasedIdentifierFromObject(GameObject g)
    // {
    //     return CreateNewTimeAndInstanceBasedIdentifier(g.GetInstanceID());
    // }

    public static ulong GetHashFromString(this string s)
    {
        // var bytes = ObjectIDExtensions.GetBytesFromString(s);
        MD5 md5 = MD5.Create();
        md5.Initialize();
        md5.ComputeHash(Encoding.UTF8.GetBytes(s));
        byte[] result = md5.Hash;
        ulong u = BitConverter.ToUInt64(result, 1) >> 1;
        // long r = BitConverter.ToInt64(result, 0);
        long l = (long)u;
        //        Debug.Log(s.Length + " rxxx esult u " + u + " / " + u.ToStringAsHex() + " xxxxas long " + l + " / " + ((ulong) l).ToStringAsHex() + "   r trim ");
        return u;
    }

    // public static ulong GetObjectID(this GameObject source)
    // {
    //     var obj = source.GetComponent<ObjectID>();
    //     if (obj == null) return 0;
    //     return obj.identifier;
    // }
    // public static ulong GetObjectIDForced(this Component source)
    // {
    //     return GetObjectIDForced(source.transform);
    // }
    public static ulong GetObjectID(this Transform source)
    {
        var obj = source.GetComponent<ObjectID>();

        if (obj == null) return 0;
        return obj.identifier;
    }
    // public static ulong GetObjectIDForced(this Transform source)
    // {
    //     var obj = source.AddOrGetComponent<ObjectID>();
    //     return obj.identifier;
    // }
    public static string ToStringAsHex(this ulong identifier, bool reverse = true)
    {
        var bytes = BitConverter.GetBytes(identifier);
        var sb = new System.Text.StringBuilder();
        for (int i = bytes.Length - 1; i >= 0; i--)
        {
            sb.Append("[");
            sb.Append(bytes[i].ToString("X2"));
            sb.Append("]");
        }
        return sb.ToString();
    }
    static readonly float bytdiv = 1.3f / System.Byte.MaxValue; // skewed
    public static string ToNiceString(this ulong identifier)
    {
        return ToStringAsHex(identifier);
    }
    public static Color GetColorFromByte(byte thisbyte)
    {
        float b = bytdiv * (((byte)(thisbyte >> 4)) << 4);
        float g = bytdiv * ((byte)(thisbyte << 4));
        // thisColor.g =0;
        float r = b * g;
        return new Color(r, g, b);
    }

    static byte FromString(string s)
    {
        return (byte)(((char)s[0] - '0') << 4 + ((char)(s[1]) - '0'));
    }

    //https://stackoverflow.com/questions/321370/how-can-i-convert-a-hex-string-to-a-byte-array
    public static byte[] StringToByteArrayFastest(string hex)
    {
        if (hex.Length % 2 == 1)
            throw new Exception("The binary key cannot have an odd number of digits '" + hex + "'");

        byte[] arr = new byte[hex.Length >> 1];

        for (int i = 0; i < hex.Length >> 1; ++i)
        {
            arr[i] = (byte)((GetHexVal(hex[i << 1]) << 4) + (GetHexVal(hex[(i << 1) + 1])));
        }

        return arr;
    }

    public static int GetHexVal(char hex)
    {
        int val = (int)hex;
        //For uppercase A-F letters:
        //return val - (val < 58 ? 48 : 55);
        //For lowercase a-f letters:
        //return val - (val < 58 ? 48 : 87);
        //Or the two combined, but a bit slower:
        return val - (val < 58 ? 48 : (val < 97 ? 55 : 87));
    }
    public static ulong FromFingerPrint(this string identifier)
    {
        if (identifier[0] == '[')
            identifier = identifier.Substring(1, identifier.Length - 2);
        identifier = identifier.ToLower();
        var bytes = StringToByteArrayFastest(identifier);
       
        return ((System.UInt64)System.BitConverter.ToUInt32(bytes, 0)) << 32; // << 32
    }
    public static string ToFingerprintString(this ulong identifier, bool colorful = false)
    {
        if (identifier == 0) return "[        no.id       ]";

        var bytes = BitConverter.GetBytes(identifier);
        string s = "[";
        // for (int i = 7; i >= 4; i--)
        for (int i = 4; i <= 7; i++)
        {
            byte thisbyte = bytes[i];
            if (colorful)
            {
                Color thisColor = GetColorFromByte(thisbyte);
                s += " " + thisbyte.ToString("X2").MakeColor(thisColor);
            }
            else
            {
                s += thisbyte.ToString("X2");
            }
        }
        s += "]";
        byte rest = (byte)(bytes[0] + bytes[1] + bytes[2] + bytes[3]);
        if (colorful)
            s += " [" + rest.ToString("X2").MakeColor(GetColorFromByte(rest)) + "] ";
        //else 
        //  s += " [" + rest.ToString("X2") + "] ";
        return colorful ? s.Small() : s;
        // if (identifier == 0) return "[no id]";
        // var bytes = BitConverter.GetBytes(identifier);
        // string rs = "-";
        // int start = 2;
        // int end = 6;

        // int index = 0;
        // float byteDiv = 1f / 256;

        // for (int i = end; i >= start; i--)
        // {
        //     float r = byteDiv * bytes[i];
        //     float g = byteDiv * bytes[(i + 1) % 8];
        //     float b = byteDiv * bytes[(i + 2) % 8];
        //     rs += (bytes[i].ToString("X2") + "-").MakeColor(new Color(r, g, b));
        // }
        // return rs;
    }
    static readonly Color faded = Color.white * .35f;
    static readonly Color faded2 = Color.white * .85f;
    static readonly string leftbracketa = "[".MakeColor(faded);
    static readonly string rightbrackera = "]".MakeColor(faded);
    static readonly string leftbracketb = "[".MakeColor(faded2);
    static readonly string rightbrackerb = "]".MakeColor(faded2);

    public static string ToColorfulString(this byte[] bytes)
    {
        string rs = "";
        for (int i = bytes.Length - 1; i >= 0; i--)
        {

            bool odd = i % 4 < 2;
            Color color = GetColorFromByte(bytes[i]);
            string bytestring = (bytes[i].ToString("X2")).MakeColor(color);
            rs += (odd ? leftbracketa : leftbracketb) + bytestring + (odd ? rightbrackera : rightbrackerb);
        }
        return rs.Small();
    }
    public static string ToColorfulString(this ulong identifier)
    {
        if (identifier == 0) return "[        no id       ]";
        var bytes = BitConverter.GetBytes(identifier);
        return ToColorfulString(bytes);

    }

    public static string ToColorfulString(this int intId)
    {
        // if (identifier == 0) return "[        no id       ]";
        var bytes = BitConverter.GetBytes(intId);
        return ToColorfulString(bytes);

    }
    public static string ToStringAsHexA(byte[] bytes, bool reverse = true)
    {
        string idStringPartA = "";
        if (reverse)
            for (int i = bytes.Length - 1; i >= bytes.Length / 2; i--)
                idStringPartA += "[" + bytes[i].ToString("X2") + "]";
        else
            for (int i = 0; i < bytes.Length / 2; i++)
                idStringPartA += "[" + bytes[i].ToString("X2") + "]";
        return idStringPartA;
    }
    public static string ToStringAsHexB(byte[] bytes, bool reverse = true)
    {
        string idStringPartB = "";
        if (reverse)
        {
            for (int i = bytes.Length / 2 - 1; i >= 0; i--)
                idStringPartB += "[" + bytes[i].ToString("X2") + "]";
        }
        else
        {
            for (int i = bytes.Length / 2; i < bytes.Length; i++)
                idStringPartB += "[" + bytes[i].ToString("X2") + "]";
        }
        return idStringPartB;
    }

    // public static string GetLabelOrName(this Transform source)
    // {
    //     return GetLabelOrName(source.gameObject);
    // }
    // public static string GetLabelOrName(this GameObject source)
    // {
    //     var labeler = source.GetComponent<IProvideLabel>();
    //     if (labeler != null) return labeler.label;
    //     return source.name;
    // }
    // public static string GetLabelOrName(this Component source)
    // {
    //     var labeler = source.GetComponent<IProvideLabel>();
    //     if (labeler != null) return labeler.label;
    //     return source.name;
    // }
    // public static string GetLabelOrName(this ulong source)
    // {
    //     var obj = source.FindObject();
    //     if (obj == null) return null;
    //     var labeler = obj.GetComponent<IProvideLabel>();
    //     if (labeler != null) return "L" + labeler.label;
    //     return obj.name;
    // }

#if UNITY_EDITOR
    [UnityEditor.MenuItem("Tools/ObjectID/reAssign New ObjectID to existing objects")]
    static void AssignNewObjectID()
    {
        var allobjs = Resources.FindObjectsOfTypeAll(typeof(ObjectID)) as ObjectID[];
        Debug.Log("found " + allobjs.Length);
        foreach (var v in allobjs)
        {
            if (v.hideFlags == HideFlags.None)
            {
                UnityEditor.Undo.RecordObject(v, "new id");
                v.GetNewId();
            }
        }
    }

    static int addedCounter;
    static int foundCounter;
    //static int incrementedOnIDGeneration;
    [MenuItem("Tools/ObjectID/Add objectIDs to selected")]
    static void AddToAllSelected()
    {
        addedCounter = 0;
        foundCounter = 0;
        foreach (var s in Selection.gameObjects)
        {
            // Debug.Log("adding to " + s.name);
            AddToTransformAndChildren(s.transform);
        }
        PrintSummary(addedCounter, Selection.gameObjects.Length, foundCounter);
    }
#if LAYOUTPANEL
    [MenuItem("Tools/ObjectID/Add objectIDs to Renderers and panels")]
    static void AddToAllSelectedWithCompnentList()
    {
        addedCounter = 0;
        foundCounter = 0;
        foreach (var s in Selection.gameObjects)
        {
            AddToTransformAndChildren(s.transform, typeof(MeshRenderer), typeof(LayoutPanel), typeof(Slider), typeof(Toggle), typeof(BrownianMotionZ));
        }

    }
#endif
    static void PrintSummary(int added, int total, int skipped = 0)
    {
        Debug.Log("added " + added + " total= " + total + " skipped =" + foundCounter);
    }

    [MenuItem("Tools/ObjectID/Add objectIDs all objects")]
    static void AddToEVERYTING()
    {
        zBench.Start("add to all");
        var allGameObjects = Resources.FindObjectsOfTypeAll<GameObject>() as GameObject[];
        addedCounter = 0;
        foundCounter = 0;
        foreach (var g in allGameObjects)
        {
            if (!zBench.PrefabModeIsActive(g))
            {
                if (g.GetComponent<ObjectID>() == null)
                {
                    UnityEditor.Undo.AddComponent(g, typeof(ObjectID));
                    addedCounter++;
                }
                else
                {
                    foundCounter++;
                }
            }
        }
        {

            addedCounter++;
        }
        zBench.End("add to all");
        PrintSummary(addedCounter, allGameObjects.Length, foundCounter);
    }
    static void AddToTransformAndChildren(Transform t, params System.Type[] types)
    {
        // Debug.Log($" tyl { types.Length}");
        for (int i = 0; i < t.childCount; i++)
        {
            AddToTransformAndChildren(t.GetChild(i), types);
        }

        if (t.gameObject.GetComponent<ObjectID>() == null)
        {
            bool hasAny = false;
            for (int k = 0; k < types.Length; k++)
            {
                if (t.gameObject.GetComponent(types[k]) != null)
                {
                    hasAny = true;
                    Debug.Log("found wanted type");
                    break;
                }
            }
            if (hasAny)
            {
                UnityEditor.Undo.AddComponent(t.gameObject, typeof(ObjectID));
                addedCounter++;
            }
        }
        else
            foundCounter++;

    }
#endif
}
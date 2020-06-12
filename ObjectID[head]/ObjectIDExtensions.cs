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

    // public static class ObjectIdEtensions
    // {
public static ulong Compact(this ulong id)
{
    return ((ulong)((Int32)( id ^ (id>>32)))<<32);
}
    static byte[] bytes = new byte[8];
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
        if (source == null) return 0;
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

    public static ulong ObtainRandomme()
    {
        random.NextBytes(bytes);
        return BitConverter.ToUInt64(bytes, 0);
    }
    public static ulong GetWhatevID(GameObject source)
    {
#if UNITY_EDITOR
        ulong newidentifier = ObjectIDExtensions.CreateNewTimeAndInstanceBasedIdentifierFromObject(source); // in editopr we take time and instance id
#else
        ulong newidentifier = ObjectIDExtensions.CreateHierarchyBasedIdentified(source);
#endif
        return newidentifier;
    }
    public static bool CreateAndValidateIdentifier(ObjectID obj)
    {
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
                    Debug.Log(obj.name + " failed registering " + newidentifier.ToFingerprintString() + " occupied by " + newidentifier.FindTransform().name);
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
    public static int yetanothercounter = 0;
    public static ulong CreateNewTimeAndInstanceBasedIdentifier(Int32 instanceid)
    {
        ulong newId = ((ulong) (System.DateTime.Now.Ticks >> 16 & (System.Int32.MaxValue >> 1))) << 32; //^ g.GetInstanceID()
        // return
        //      newId=newId|(UInt64)(long)(yetanothercounter<<56);
        ulong truncatedInstancid = (ulong) instanceid; // & 0b111111111111111;
        //incrementedOnIDGeneration += 2;
        //truncatedInstancid=(ulong)UnityEngine.Random.Range(0,System.Int32.MaxValue);
        yetanothercounter++;

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
        newId = newId | (UInt32) (truncatedInstancid | (ulong)(long) yetanothercounter);
        // newId = 0 |( (ulong) ((yetanothercounter) << 16)) |(ulong)(long)( truncatedInstancid);

        // UInt32 x = (UInt32) (truncatedInstancid + (int) yetanothercounter);
        //   Int64 y = (Int64) (truncatedInstancid + (int) yetanothercounter);
        //   Debug.Log(newId.ToFingerprintString() + " creatbg   x  " + ((ulong) (x)).ToFingerprintString() + "     " + "  y  " + ((ulong) (y)).ToStringAsHex() + "     ");
        //           Debug.Log(" tick " + newId.ToFingerprintString() + "   x  " + newId.ToStringAsHex() + "     " + newId);

        return newId;
    }
    public static ulong CreateNewTimeAndInstanceBasedIdentifierFromObject(GameObject g)
    {
        return CreateNewTimeAndInstanceBasedIdentifier(g.GetInstanceID());
    }

    public static ulong GetHashFromString(this string s)
    {
        // var bytes = ObjectIDExtensions.GetBytesFromString(s);
        MD5 md5 = MD5.Create();
        md5.Initialize();
        md5.ComputeHash(Encoding.UTF8.GetBytes(s));
        byte[] result = md5.Hash;
        ulong u = BitConverter.ToUInt64(result, 1) >> 1;
        // long r = BitConverter.ToInt64(result, 0);
        long l = (long) u;
        //        Debug.Log(s.Length + " rxxx esult u " + u + " / " + u.ToStringAsHex() + " xxxxas long " + l + " / " + ((ulong) l).ToStringAsHex() + "   r trim ");
        return u;
    }

    // public static ulong GetObjectID(this GameObject source)
    // {
    //     var obj = source.GetComponent<ObjectID>();
    //     if (obj == null) return 0;
    //     return obj.identifier;
    // }
    public static ulong GetObjectIDForced(this Component source)
    {
        return GetObjectIDForced(source.transform);
    }
    public static ulong GetObjectID(this Transform source)
    {
        var obj = source.GetComponent<ObjectID>();

        if (obj == null) return 0;
        return obj.identifier;
    }
    public static ulong GetObjectIDForced(this Transform source)
    {
        var obj = source.AddOrGetComponent<ObjectID>();
        return obj.identifier;
    }
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
    public static string ToNiceString(this ulong identifier)
    {
        return ToStringAsHex(identifier);
    }
    public static string ToFingerprintString(this ulong identifier)
    {
        return ToColorfulString(identifier);
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
    public static string ToColorfulString(this ulong identifier, int steps = 3)
    {
        if (identifier == 0) return "[        no id       ]";
        var bytes = BitConverter.GetBytes(identifier);
        string rs = "";
        // float byteDiv = 1f / (256);
        Color faded = Color.white * .35f;
        string leftbracketa = "[".MakeColor(faded);
        string rightbrackera = "]".MakeColor(faded);
        faded = Color.white * .85f;
        string leftbracketb = "[".MakeColor(faded);
        string rightbrackerb = "]".MakeColor(faded);
        // float r = .5f;
        // float g = .5f;
        // float b = .5f;
        Color Start = new Color32((byte) (bytes[0] ^ bytes[3]), bytes[1], bytes[2], 255);
        Color End = new Color32(bytes[7], (byte) (bytes[6] ^ bytes[3]), bytes[5], 255);

        for (int i = 7; i >= 0; i--)
        {
            // if (i < 4)
            //     r = byteDiv * bytes[i / 4];
            // if (i > 5)
            //     g = byteDiv * bytes[((i) / 4 + 2) % 8];
            // b = byteDiv * bytes[((i) / 4 + 5) % 8];

            // float bri=bytes[i] * byteDiv * .6f + .2f;
            float bri = bytes[i] * 1.5f / 256;
            bri *= bri;
            bri *= 2;
            Color color1 = Color.Lerp(Start, End, i * (1 / 8f)); //(new Color(r, g, b)
            Color color = color1.SaturationAndBrigntessAdjust(.6f, bri);
            bool odd = i % 4 < 2;
            string bytestring = (bytes[i].ToString("X2")).MakeColor(color);
            rs += (odd ? leftbracketa : leftbracketb) + bytestring + (odd ? rightbrackera : rightbrackerb);
        }
        return rs;
    }
    public static string ToStringAsHexA(byte[] bytes, bool reverse)
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
    public static string ToStringAsHexB(byte[] bytes, bool reverse)
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

    // [MenuItem("Tools/ObjectID/Randomize objectid seed")]

    // static void NewId()
    // {
    //     ObjectID.Seed(UnityEngine.Random.Range(0, System.Int32.MaxValue));
    //     Debug.Log("randomized seed for objid ");
    // }
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
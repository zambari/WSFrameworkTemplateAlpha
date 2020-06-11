using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// v.02 more finds

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
}
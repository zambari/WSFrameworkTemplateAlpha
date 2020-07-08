using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class TransformNodeInfo
{
    public string name;
    public ulong id;
    // public System.Int32 flags;
    public string idFingerPrint;
    public int layer;
    public int hideFlags;
    public Vector3 localPosition;
    public Vector3 localScale;
    public Quaternion localRotation;
    public int childCount;
    public bool active;
    public TransformNodeInfo(Transform src)
    {
        layer = src.gameObject.layer;
        name = src.name;
        id = src.GetObjectID(true);
        if (id == 0)
        {
            src.AddOrGetComponent<ObjectID>().GetNewId();
        }
        hideFlags = (int) src.gameObject.hideFlags;
        localRotation = src.localRotation;
        localPosition = src.localPosition;
        localScale = src.localScale;
        childCount = src.childCount;
        idFingerPrint = id.ToFingerprintString();
        this.active = src.gameObject.activeSelf;
    }
    public void Apply(Transform dst)
    {
        dst.localPosition = localPosition;
        dst.localRotation = localRotation;
        dst.localScale = localScale;
    }
}
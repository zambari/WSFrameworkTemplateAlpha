using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class BasicComponentInfo // : WSFrameworkMessage
{
    public string name;
    public bool enabled;
    public int componentID;
    static Dictionary<int, Component> componentDict;
    static int nextComponentID { get { return lastComponentID++; } }
    static int lastComponentID;
    public static Component GetFromComponetId(int id)
    {
        Component c = null;
        if (componentDict != null && componentDict.TryGetValue(id, out c))
        {

        }
        return c;
    }
    public static void ReleseComponent(int id)
    {
        if (componentDict != null) componentDict.Remove(id);
    }
    public BasicComponentInfo(Component c)
    {
        if (componentDict == null)
        {
            componentDict = new Dictionary<int, Component>();
        }
        name = c.GetType().ToString();
        MonoBehaviour mono = c as MonoBehaviour;
        enabled = mono == null?true : mono.enabled;
        componentID = nextComponentID;
        componentDict.Add(componentID, c);
    }
}

[System.Serializable]
public class GameObjectInfo // : WSFrameworkMessage
{
    public string name;
    public ulong id;
    public string idFingerPrint;
    public bool isActive;
    public List<BasicComponentInfo> componentInfos;
    public GameObjectInfo(GameObject g)
    {
        GetInfo(g);
    }
    void GetInfo(GameObject g)
    {
        name = g.name;
        id = g.GetID();
        idFingerPrint = id.ToFingerprintString(false);
        isActive = g.activeSelf;
        var comps = g.GetComponents<Component>();
        componentInfos = new List<BasicComponentInfo>();
        for (int i = 0; i < comps.Length; i++)
        {
            if (ShouldInclude(comps[i]))
                componentInfos.Add(new BasicComponentInfo(comps[i]));
        }
    }
    bool ShouldInclude(Component c)
    {
        if (c is ObjectID) return false;
        if (c is CanvasRenderer) return false;

        return true;
    }
    public GameObjectInfo(Transform t)
    {
        GetInfo(t.gameObject);
    }
}
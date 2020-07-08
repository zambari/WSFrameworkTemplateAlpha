using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// public class WSHierarchyRequest : WSFrameworkMessage
// {
// 	public string _type = "hierarchyRequest";
// 	public override string type { get { return _type; } }
// 	public LayerMask layerMask = System.Int32.MaxValue;
// 	public long root = 0;
// }



[System.Serializable]
public class WSHierarchyResponse : WSFrameworkMessage
{
    // public string _type = "hierarchyResponse";
    public ulong id = 0;
    public string name;
    public string sceneName;

    // public override string type { get { return _type; } }
    // public long root2 = 0;
    public List<TransformNodeInfo> nodes = new List<TransformNodeInfo>();
    public WSHierarchyResponse()
    {

    }
    public WSHierarchyResponse(Transform transform, ulong id)
    {
        if (id == 0) id = transform.GetID();
        name = transform.name;
        this.id = id;

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform thischild = transform.GetChild(i);
            nodes.Add(new TransformNodeInfo(thischild));
        }
    }
}


///////////

public abstract class WSFrameworkMessage
{
    // public abstract string type { get; }

}
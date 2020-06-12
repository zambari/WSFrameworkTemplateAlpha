using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class WSHierarchyResponse : WSFrameworkMessage
{
	public string _type = "hierarchyResponse";
	public override string type { get { return _type; } }
	public ulong root = 0;
	public long root2 = 0;
	public List<TransformNodeInfo> nodes = new List<TransformNodeInfo>();
}

[System.Serializable]
public class GameObjectInfo // : WSFrameworkMessage
{
	public string name;
	public ulong id;
	public bool isActive;
	public string[] componentNames;
	public GameObjectInfo(GameObject g)
	{
		GetInfo(g);
	}
	void GetInfo(GameObject g)
	{
		name = g.name;
		id = g.GetID();
		isActive = g.activeSelf;
		var comps = g.GetComponents<Component>();
		componentNames = new string[comps.Length];
		for (int i = 0; i < comps.Length; i++)
			componentNames[i] = comps[i].GetType().ToString();
	}
	public GameObjectInfo(Transform t)
	{
		GetInfo(t.gameObject);
	}
}

[System.Serializable]
public class TransformNodeInfo
{
	public string name;
	public ulong objectId;
	public System.Int32 flags;
	public int layer;
	public int hideFlags;
	public TransformNodeInfo(Transform src)
	{
		layer = src.gameObject.layer;
		name = src.name;
		objectId = src.GetObjectID(true);
		hideFlags = (int) src.gameObject.hideFlags;
	}
}

///////////

public abstract class WSFrameworkMessage
{
	public abstract string type { get; }

}
public class WSHierarchyRequest : WSFrameworkMessage
{
	public string _type = "hierarchyRequest";
	public override string type { get { return _type; } }
	public LayerMask layerMask = System.Int32.MaxValue;
	public long root = 0;
}
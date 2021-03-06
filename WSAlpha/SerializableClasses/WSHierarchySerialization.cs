﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class WSHierarchyResponse: WSFrameworkMessage
{	public string _type = "hierarchyResponse";
	public override string type { get { return _type; } }
	public ulong root=0;
	public long root2=0;
	public List<TransformNodeInfo> transfoms = new List<TransformNodeInfo>();
}

[System.Serializable]
public class TransformNodeInfo
{
	public string name;
	public ulong objectId;
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
	public long root=0;
}
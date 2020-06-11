using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HierachyFiller : ScrolPooledControllerBase
{
	List<TransformNodeInfo> nodes;
	WSHierarchyResponse repsonse;
	public void UpdateNodes(WSHierarchyResponse repsonse)
	{
		this.repsonse = repsonse;
		nodes = repsonse.nodes;
		scrollPooled.InitList(this, repsonse.nodes.Count);
		// Debug.Log("updating nodes");
	}
	public override int GetCount()
	{
		return repsonse.nodes.Count;
	}

	public override void OnFillItem(int index, GameObject go)
	{
		var item = go.GetComponent<ListItem>();
		item.label = repsonse.nodes[index].name + " hide " + repsonse.nodes[index].hideFlags;
		Debug.Log("filling " + item.label);
	}

}
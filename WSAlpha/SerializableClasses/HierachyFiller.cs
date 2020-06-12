using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HierachyFiller : ScrolPooledControllerBase
{
	List<TransformNodeInfo> nodes;
	WSHierarchyResponse repsonse;
	public WSObjectReferencingClient itemSelectionReciever;
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
		TransformNodeInfo node = repsonse.nodes[index];
		var item = go.GetComponent<ListItem>();
		var b = item.GetComponentInChildren<Button>();
		item.ClearListeners();
		if (itemSelectionReciever != null)
			item.SetCallback(() =>
			{
				itemSelectionReciever.OnNodeClicked(node);
			});
		//	DestroyImmediate(b);
		item.label = node.name + " / " + (node.objectId == 0 ? "" : node.objectId.ToFingerprintString());
		// 	Debug.Log("filling " + item.label);
	}

}
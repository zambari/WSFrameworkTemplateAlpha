using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HierachyFiller : ScrolPooledControllerBase
{
	List<TransformNodeInfo> nodes;
	WSHierarchyResponse repsonse;
	public WSComponentClient itemSelectionReciever;
	public void UpdateRoots(WSHierarchyResponse repsonse)
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
		item.label = node.name + " / " + (node.id == 0 ? "" : node.id.ToFingerprintString());
		// 	Debug.Log("filling " + item.label);
	}

}
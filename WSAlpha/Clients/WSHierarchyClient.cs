using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityOSC;
using WebSocketSharp;
//zbr 2020

public class WSHierarchyClient : WSOSCClient
{
	public Transform content;
	protected override void OnOSCMessage(OSCMessage message)
	{
		// DebugClient("recienig mes addr " + message.Address + " type " + message.typeTag);
		if (message.Address == WSHierarchyService.oscRequest)
		{
			if (message.typeTag[1] == 's')
			{
				string payload = message.GetString(0);
				Debug.Log("decoding roots "+payload);

				var response = JsonUtility.FromJson<WSHierarchyResponse>(payload);
				if (response != null)
				{
					UpdateNodes(response);
				}
				else
				{
					Debug.Log("inalid response " + payload);
				}
			}
			else
			{
				Debug.Log("no string payload");
			}
		}
		else
		{
			DebugClient("unknown address " + message.Address);
		}
	}
	void UpdateNodes(WSHierarchyResponse repsonse)
	{
		var filler=GetComponent<HierachyFiller>();
		filler.UpdateNodes(repsonse);
		// Debug.Log("updating nodes");
	}

	[ExposeMethodInEditor]
	public void SendRequest()
	{
		OSCMessage message = new OSCMessage(WSHierarchyService.oscRequest);
		Send(message);
	}
}
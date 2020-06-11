using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityOSC;
using WebSocketSharp;

public class WSHierarchyService : WSOSCService
{

	static public readonly string oscRequest = "/roots";
	protected override void OnOSCMessage(OSCMessage message, WSServiceBehaviour beh)
	{
		//    	DebugService(" got :" + s.Data);

		if (message.Address == oscRequest)
		{
			SendResponse(beh);
		}
		else
		{
			DebugService("unknowna dress " + message.Address);
		}

		// var obj = JsonUtility.FromJson<WSFrameworkMessage>(s.Data);
		// if (obj.type != "hierarchyRequest")
		// 	return;
		// var req = JsonUtility.FromJson<WSHierarchyRequest>(s.Data);
		// Debug.Log("req.root" + req.root);

		// SendResponse();

	}

	void SendResponse(WSServiceBehaviour beh)
	{
		var stopwatch = new System.Diagnostics.Stopwatch();
		stopwatch.Start();
		var response = new WSHierarchyResponse();
		var alltransforms = Resources.FindObjectsOfTypeAll<Transform>() as Transform[];
		var transformList = new List<Transform>();
		foreach (var t in alltransforms)
		{
			if (t.parent == null) // t.hideFlags == HideFlags.None // layer
			{
				transformList.Add(t);
			}

		}
		response.nodes = new List<TransformNodeInfo>();
		for (int i = 0; i < transformList.Count; i++)
		{
			response.nodes.Add(new TransformNodeInfo(transformList[i]));
		}
		string serializerResponse = JsonUtility.ToJson(response);
		OSCMessage message = new OSCMessage(oscRequest);
		message.Append(serializerResponse);
		beh.Send(message);
		Debug.Log(" our message has " + message.BinaryData.Length + " bytes");
		stopwatch.Stop();
		DebugService("found " + transformList.Count + " root transforms");
		DebugService("Ellapses " + stopwatch.ElapsedMilliseconds + " ms /" + stopwatch.ElapsedTicks + "/ticks");
	}

}
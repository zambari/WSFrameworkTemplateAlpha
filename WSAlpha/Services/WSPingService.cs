using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;

public class WSPingService : WSServiceBase
{

	// Use this for initialization

	// [SerializeField]
	// protected string _serviceName;
	// public override string serviceName { get { return _serviceName; } } // "/ping";

	protected override void Reset()
	{
		base.Reset();
		_serviceName = "/ping";
	}
	protected override void OnMessageDequeue(WSServiceBehaviour beh, MessageEventArgs s)
	{
		DebugService(" ping recieved got :" + s.RawData.Length + " s ");
		DebugService(" ping recieved got :" + s.RawData.Length + " s ");
		string croppedstring = s.Data;
		if (croppedstring != null)
		{
			if (croppedstring.Length > 100) croppedstring = croppedstring.Substring(0, 100);
			int pingstringStart = -1; // not found
			pingstringStart = croppedstring.IndexOf("ping");
			if (pingstringStart < 0) pingstringStart = croppedstring.IndexOf("PING");
			if (pingstringStart < 0) pingstringStart = croppedstring.IndexOf("ping");
			if (pingstringStart > 0)
			{
				croppedstring = croppedstring.Substring(0, pingstringStart + 1) + '0' + croppedstring.Substring(pingstringStart + 2); //) [pingstringStart + 1] = '0';
			}
			beh.SendString(croppedstring);

			DebugService(" rent response :'" + croppedstring + "'");
		}
		else
		{
			beh.SendString("pongish");
		}
	}
	// var obj = JsonUtility.FromJson<WSFrameworkMessage>(s.Data);
	// if (obj.type != "hierarchyRequest")
	// 	return;
	// var req = JsonUtility.FromJson<WSHierarchyRequest>(s.Data);
	// Debug.Log("req.root" + req.root);

	// SendResponse();

	//	var allRoots=new 
	//		SendFrame();
}
// void Start()
// {
// 	server.AddService("/ping",Initializer);

// }
// void SendResponse()
// {
// 	var stopwatch = new System.Diagnostics.Stopwatch();
// 	stopwatch.Start();
// 	var response = new WSHierarchyResponse();
// 	var alltransforms = Resources.FindObjectsOfTypeAll<Transform>() as Transform[];
// 	var transformList = new List<Transform>();
// 	foreach (var t in alltransforms)
// 	{
// 		if (t.parent == null)
// 		{
// 			transformList.Add(t);

// 		}

// 	}
// 	stopwatch.Stop();
// 	DebugService("found " + transformList.Count + " root transforms");
// 	DebugService("Ellapses " + stopwatch.ElapsedMilliseconds + " ms /" + stopwatch.ElapsedTicks + "/ticks");
// }

// }
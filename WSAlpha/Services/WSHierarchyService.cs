using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;

public class WSHierarchyService : WSServiceBase
{

	// Use this for initialization
	protected override void OnMessageDequeue(WSServiceBehaviour beh,MessageEventArgs s)
	{
		DebugService(" got :" + s.Data);

		var obj = JsonUtility.FromJson<WSFrameworkMessage>(s.Data);
		if (obj.type != "hierarchyRequest")
			return;
		var req = JsonUtility.FromJson<WSHierarchyRequest>(s.Data);
		Debug.Log("req.root" + req.root);

		SendResponse();

		//	var allRoots=new 
		//		SendFrame();
	}

	void SendResponse()
	{
		var stopwatch = new System.Diagnostics.Stopwatch();
		stopwatch.Start();
		var response = new WSHierarchyResponse();
		var alltransforms = Resources.FindObjectsOfTypeAll<Transform>() as Transform[];
		var transformList = new List<Transform>();
		foreach (var t in alltransforms)
		{
			if (t.parent == null)
			{
				transformList.Add(t);

			}

		}
		stopwatch.Stop();
		DebugService("found " + transformList.Count + " root transforms");
		DebugService("Ellapses " + stopwatch.ElapsedMilliseconds + " ms /" + stopwatch.ElapsedTicks + "/ticks");
	}

}
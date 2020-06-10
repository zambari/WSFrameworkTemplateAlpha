
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;
//zbr 2020

public class WSFileSystemService : WSServiceBase
{
protected override void Reset()
	{
		base.Reset();
		_serviceName = "/Files";
	}
	// Use this for initialization
	protected override void OnMessageDequeue(WSServiceBehaviour beh,MessageEventArgs s)
	{
//		SendFrame();
	}


}
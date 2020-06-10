using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityOSC;
using WebSocketSharp;
public class WSPointCloudServer : WSServiceBase
{
	//	RealsenseHandler2 handler2;

	protected override void OnMessageDequeue(WSServiceBehaviour beh, MessageEventArgs message)
	{
		Debug.Log("pointcloud server " + message.RawData.Length);
	}

	protected override void Start()
	{
		base.Start();
		// if (handler2 == null) handler2 = GetComponent<RealsenseHandler2>();
		// handler2.onFrameRecieved += OnFrame;

	}
	// void OnFrame()
	// {
	// 	OSCMessage message = new OSCMessage("/depth");
	// 	message.Append(handler2.textureDimensions.x);
	// 	message.Append(handler2.textureDimensions.y);
	// 	message.Append(handler2.GetBytes());
	// 	BroacdcastBytes(message.BinaryData);
	// }

}
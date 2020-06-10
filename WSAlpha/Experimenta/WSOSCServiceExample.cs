using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityOSC;
using WebSocketSharp;

public class WSOSCServiceExample : WSServiceBase
{

	// Use this for initialization

	protected override void OnMessageDequeue(WSServiceBehaviour beh, MessageEventArgs message)
	{
		OSCPacket packet = OSCMessage.Unpack(message.RawData);
		DebugService("service recieved osc " + packet.Address + " " + packet.typeTag);
	}

	[ExposeMethodInEditor]
	void BroadcastTesMessage()
	{
		OSCMessage message = new OSCMessage("/testmesg");
		BroacdcastBytes(message.BinaryData);
	}

}
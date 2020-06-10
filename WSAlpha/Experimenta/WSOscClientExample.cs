using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityOSC;
using WebSocketSharp;

[RequireComponent(typeof(WSTargetAddress))]
public class WSOscClientExample : WSClientBase
{

	protected override void OnMessageDequeue(MessageEventArgs msg)
	{
		OSCPacket packet= OSCMessage.Unpack(msg.RawData);
		DebugClient("recieved osc "+packet.Address+" "+packet.typeTag);
	}

	[ExposeMethodInEditor] void SendTest()
	{
		OSCMessage message = new OSCMessage("Test message");
		message.Append(Random.value);
		Send(message.BinaryData);
	}

}
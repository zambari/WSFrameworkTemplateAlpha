using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityOSC;
using Z;

public abstract class WSOSCClient : WSClientBase
{
	protected abstract void OnOSCMessage(OSCMessage message);

	protected override void OnMessageDequeue(WebSocketSharp.MessageEventArgs message)
	{
		// statsSumary.AddBytesRecieved(message.RawData.Length);
		OSCPacket oscpacket = OSCPacket.Unpack(message.RawData);

		if (oscpacket.Address == "/message")
		{
			DebugClient("recieved messaget " + oscpacket.GetString(0));
		}
		if (oscpacket != null)
		{
			OnOSCMessage(oscpacket as OSCMessage);
			//	DebugClient("recieved " + oscpacket.Address);
		}
		else
			DebugClient("unpacking osc messge failed " + message.Data);
	}
	public void SendAsync(OSCMessage msg)
	{
		Send(msg);
	}
	public void Send(OSCMessage msg)
	{
		if (ws == null)
		{
			DebugClient("socket not connected");
			return;
		}
		ws.SendAsync(msg.BinaryData, null);
	}

}
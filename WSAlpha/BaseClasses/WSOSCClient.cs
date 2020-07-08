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

		if (oscpacket.Address.EndsWith("/message"))
		{
			if (oscpacket.typeTag[0] == 'u')
				DebugClient(oscpacket.Address + " MESSAGE recieved with ID:  " + oscpacket.GetULong(0));
			if (oscpacket.typeTag[0] == 's')
				DebugClient(oscpacket.Address + " MESSAGE recieved  " + oscpacket.GetString(0));
		}
		if (statsSumary.printOnRecieve)
		{
			if (oscpacket.typeTag != null && oscpacket.typeTag.Length > 1 && oscpacket.typeTag[1] == 's')
			{
				DebugClient("String payload ==========:");
				Debug.Log(oscpacket.GetString(0));
			}
			else
			{
				DebugClient(" Recieved " + oscpacket.ToReadableString());

			}
		}
		OnOSCMessage(oscpacket as OSCMessage);
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
		if (statsSumary.printOnSend)
		{
			DebugClient("sending " + msg.ToReadableString());
		}
		var bytes = msg.BinaryData;
		statsSumary.AddBytesSent(bytes.Length);
		ws.SendAsync(bytes, null);
	}

}
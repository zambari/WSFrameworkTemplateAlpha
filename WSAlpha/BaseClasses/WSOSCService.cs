using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityOSC;

public abstract class WSOSCService : WSServiceBase
{
	protected abstract void OnOSCMessage(OSCMessage message, WSServiceBehaviour beh);

	protected override void OnMessageDequeue(WSServiceBehaviour beh, WebSocketSharp.MessageEventArgs message)
	{
		OSCPacket oscpacket = OSCPacket.Unpack(message.RawData);
		// if (oscpacket.typeTag != null && oscpacket.typeTag.Length > 1 && oscpacket.typeTag[1] == 's')
		// {
		// 	DebugService("String payload ==========:");
		// 	Debug.Log(oscpacket.GetString(0));
		// }
		if (oscpacket != null)
		{
			if (stats.printOnRecieve)
			{
				DebugService("Recieved " + oscpacket.ToReadableString());
			}
			OnOSCMessage(oscpacket as OSCMessage, beh);
		}
		else
			DebugService("unpacking osc messge failed " + message.Data);
	}
	public void Broacdcast(OSCMessage message)
	{
		var bytes = message.BinaryData;
		if (stats.printOnSend)
		{
			DebugService("broadcasting " + message.ToReadableString());
		}
		lock(clientHanlders)
		{
			foreach (var c in clientHanlders)
			{
				c.SendAsync(bytes, null);
				stats.AddBytesSent(bytes.Length);
			}
			if (clientHanlders.Count == 0)
				DebugService("no clients connected");
		}
	}
}
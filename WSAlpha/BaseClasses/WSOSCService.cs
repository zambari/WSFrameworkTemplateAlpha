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
		if (oscpacket.typeTag != null && oscpacket.typeTag.Length > 1 && oscpacket.typeTag[1] == 's')
		{
			DebugService("String payload ==========:");
			Debug.Log(oscpacket.GetString(0));
		}
		if (oscpacket != null)
			OnOSCMessage(oscpacket as OSCMessage, beh);
		else
			DebugService("unpacking osc messge failed " + message.Data);
	}

}
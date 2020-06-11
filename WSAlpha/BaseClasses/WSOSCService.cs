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
		if (oscpacket != null)
			OnOSCMessage(oscpacket as OSCMessage, beh);
		else
			DebugService("unpacking osc messge failed " + message.Data);
	}

}
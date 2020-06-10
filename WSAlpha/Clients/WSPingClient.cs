using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;
using Z;
public class WSPingClient : WSClientBase
{
	protected override void OnMessageDequeue(MessageEventArgs message)
	{
		DebugClient(" pingclient recieved " + message.Data);
		
	}

	[ExposeMethodInEditor]
	void SendPing()
	{
		if (ws.IsConnected)
		{
			Send(" ping ");
			DebugClient("sent");
		}
		else
		{
			DebugClient("ws not connected");
		}
	}

	[ExposeMethodInEditor]
	void SendPingAsBytes()
	{
		if (ws.IsConnected)
		{
			Send(" ping ".ToByteArray());
			DebugClient("sent");
		}
		else
		{
			DebugClient("ws not connected");
		}
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;
using WebSocketSharp.Server;

public class WSServiceBehaviour : WebSocketBehavior
{
	public Queue<MessageEventArgs> messageQueue = new Queue<MessageEventArgs>();
	public Queue<CloseEventArgs> closedQueue = new Queue<CloseEventArgs>();
	public Queue<ErrorEventArgs> errorQueue = new Queue<ErrorEventArgs>();
	public CommStats stats;
	protected override void OnMessage(MessageEventArgs e)
	{
		lock(messageQueue)
		{
			messageQueue.Enqueue(e);
		}
		//	if (stats != null) 
		stats.AddBytesRecieved(e.RawData.Length);
	}
	public override void SendBytes(byte[] data)
	{
		base.SendBytes(data);
		//	if (stats != null) 
		stats.AddBytesSent(data.Length);

	}

	public override void SendString(string data)
	{
		base.SendString(data);
		//	if (stats != null) 
		stats.AddBytesSent(data.Length);
	}
	protected override void OnClose(CloseEventArgs e)
	{
		lock(closedQueue)
		{
			closedQueue.Enqueue(e);
		}
	}
	protected override void OnError(ErrorEventArgs e)
	{
		lock(closedQueue)
		{
			errorQueue.Enqueue(e);
		}
	}
	protected override void OnOpen()
	{
		lock(closedQueue)
		{
			closedQueue.Enqueue(null);
		}
	}
}
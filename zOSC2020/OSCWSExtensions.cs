using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityOSC;
using WebSocketSharp.Server;

public static class OSCWSExtensions
{

	// Use this for initialization
	public static byte[] WrapAsOscPayload(this string address)
	{
		OSCMessage message = new OSCMessage(address);
		return message.BinaryData;
	}
	public static void Send(this WebSocketBehavior beh, OSCMessage message)
	{
		beh.SendAsync(message.BinaryData, null);
	}
	public static byte[] WrapAsOscPayload(this string payload, string address)
	{
		OSCMessage message = new OSCMessage(address);
		message.Append(payload);
		return message.BinaryData;
	}
	public static byte[] WrapAsOscPayload(this string address, int x, int y)
	{
		OSCMessage message = new OSCMessage(address);
		message.Append(x);
		message.Append(y);
		return message.BinaryData;
	}
	public static byte[] WrapAsOscPayload(this byte[] bytes, string address)
	{
		OSCMessage message = new OSCMessage(address);
		message.Append(bytes);
		return message.BinaryData;
	}
	public static byte[] WrapAsOscPayload(this string address, int x, int y, int q)
	{
		OSCMessage message = new OSCMessage(address);
		message.Append(x);
		message.Append(y);
		message.Append(q);
		return message.BinaryData;
	}
	public static byte[] WrapAsOscPayload(this float payload, string address)
	{
		OSCMessage message = new OSCMessage(address);
		message.Append(payload);
		return message.BinaryData;
	}

	public static byte[] WrapAsOscPayload(this string address, long objectID, float payload)
	{
		OSCMessage message = new OSCMessage(address);
		message.Append(objectID);
		message.Append(payload);
		return message.BinaryData;
	}

	public static byte[] WrapAsOscPayload(this string address, long objectID, string payload)
	{
		OSCMessage message = new OSCMessage(address);
		message.Append(objectID);
		message.Append(payload);
		return message.BinaryData;
	}

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityOSC;

public static class zOSCExt
{
	public static void Send(this ISendOSC sender, string address, float value)
	{
		OSCMessage message = new OSCMessage(address);
		message.Append(value);
		sender.Send(message);
	}

	public static void Send(this ISendOSC sender, string address, string targetIP, int targetPort)
	{
		OSCMessage message = new OSCMessage(address);
		message.Append(targetIP);
		message.Append(targetPort);
		sender.Send(message);
	}
	public static void Send(this ISendOSC sender, string address)
	{
		OSCMessage message = new OSCMessage(address);
		sender.Send(message);
	}
	public static void Send(this ISendOSC sender, string address, params object[] payload)
	{
		OSCMessage message = new OSCMessage(address);
		foreach (var ob in payload)
			message.Append(ob);
		sender.Send(message);
	}
}
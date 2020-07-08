using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityOSC;
using WSFrameworkConst;
using Z.Reflection;

public class WSValueClient : WSOSCClient
{
	public static WSValueClient instance;
	void Awake()
	{
		if (instance != null && instance != this)
		{
			Debug.Log("two instances");
		}
		instance = this;
	}
	public static void RequestValueChange(int valueID, float val)
	{
		OSCMessage msg = new OSCMessage(Const.valueOSC);
		msg.Append(valueID);
		msg.Append(val);
		instance.Send(msg);
	}
	// public static void RequestValueChange(int valueID, int val)
	// {
	// 	OSCMessage msg = new OSCMessage(Const.valueOSC);
	// 	msg.Append(valueID);
	// 	msg.Append(val);
	// 	instance.Send(msg);
	// }
	// public static void RequestValueChange(int valueID, string val)
	// {
	// 	OSCMessage msg = new OSCMessage(Const.valueOSC);
	// 	msg.Append(valueID);
	// 	msg.Append(val);
	// 	instance.Send(msg);
	// }

	public static void RequestValueChange(int valueID, string val)
	{
		OSCMessage msg = new OSCMessage(Const.stringvalueOSC);
		msg.Append(valueID);
		msg.Append(val);
		instance.Send(msg);
	}

	protected override void OnOSCMessage(OSCMessage message)
	{
		string address = message.Address;
		if (address.StartsWith(Const.valueOSC))
		{
			int memberId = message.GetInt(0);
			float value = message.GetFloat(1);
			var val = ValueRemote.GetRemote(memberId);
			if (val == null)
			{
				DebugClient(" remote not retrieved for id " + memberId + "  " + memberId.ToColorfulString());
			}
			else
			{
				val.UpdateClientUI(value);
				DebugClient("client updated " + memberId.ToColorfulString() + "   value=" + value);
			}
		}
		else
		if (address.StartsWith(Const.stringvalueOSC))
		{
			int memberId = message.GetInt(0);
			string value = message.GetString(1);
			var val = ValueRemote.GetRemote(memberId);
			if (val == null)
			{
				DebugClient(" remote not retrieved for id " + memberId + "  " + memberId.ToColorfulString());
			}
			else
			{
				val.UpdateClientUI(value);
				DebugClient("client updated " + memberId.ToColorfulString() + "   value=" + value);
			}
		}else
		{
			Debug.Log("some unknown msg " + address);
		}
	}

}
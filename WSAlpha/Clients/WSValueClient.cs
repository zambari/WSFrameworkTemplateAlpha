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
		if (instance!=null&&instance!=this)
		{
			Debug.Log("two instances");
		}
		instance=this;
	}
	public static void RequestValueChange(int valueID, float val)
	{
		OSCMessage msg=new OSCMessage(Const.valueOSC);
		msg.Append(valueID);
		msg.Append(val);
		instance.Send(msg);
	}

	protected override void OnOSCMessage(OSCMessage message)
	{
		string address=message.Address;
		if (address.StartsWith(Const.valueOSC))
		{
			ulong valueid=message.GetULong(0);
			float value=message.GetFloat(1);
			var val=ValueRemote.GetRemote(valueid);
			val.UpdateValue(value);
			DebugClient("is value frame "+valueid.ToColorfulString()+" "+value);
		}
		else
		{
			Debug.Log("some unknown msg "+address);
		}
	}


}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityOSC;
using WSFrameworkConst;
using Z;
using Z.Reflection;

public class WSValueService : WSOSCService
{
	static WSOSCService instance;
	void Awake()
	{
		if (instance != null && instance != this)
		{
			Debug.Log("two instances");
		}
		instance = this;
	}

	// public List<ValueProxy> activeProxies = new List<ValueProxy>();
	float minimalUpdateTime = 0.2f;
	float nextUpdateTime;
	protected override void OnOSCMessage(OSCMessage message, WSServiceBehaviour beh)
	{
		string address = message.Address;
		int memberId = message.GetInt(0);
		var proxy = ValueProxy.GetProxyFromDict(memberId);
		if (address.StartsWith(Const.valueOSC))
		{

			// tu slownik

			// DebugService("valueservice full adderss is " + address + " typetag " + message.typeTag + " vali " + memberId.ToColorfulString());
			if (proxy == null)
			{
				Debug.Log("no proxy");
			}
			else
			{
				switch (proxy.fieldType)
				{
					case MemberDescription.FieldType.FloatField:
						proxy.SetFloat(message.GetFloat(1));
						break;
					case MemberDescription.FieldType.IntField:
						if (message.GetPayloadType(1) == typeof(int))

							proxy.SetInt(message.GetInt(1));
						else
							proxy.SetInt(Mathf.FloorToInt(message.GetFloat(1) - 0.001f));
						break;
					case MemberDescription.FieldType.BoolField:
						proxy.SetBool(message.GetFloat(1) >.5f);
						break;
					case MemberDescription.FieldType.StringField:
						proxy.SetString(message.GetString(1));
						break;
					case MemberDescription.FieldType.Void:
						proxy.Invoke();
						break;
				}

			}
		}
		else
		if (address.StartsWith(Const.stringvalueOSC))
		{
			if (proxy.fieldType == MemberDescription.FieldType.StringField)
			{
				proxy.SetString(message.GetString(1));
			}
			else
				DebugService("invalid membertype");
		}
		else
		{
			DebugService("addes unknown ? " + address + "  vs " + Const.valueOSC);
		}
	}

	void UpdateProxies()
	{
		foreach (var p in ValueProxy.activeProxies)
		{
			if (p.HasValueChanged())
			{
				if (p.fieldType != MemberDescription.FieldType.StringField)
				{
					OSCMessage message = new OSCMessage(Const.valueOSC);
					message.Append(p.memberId);
					message.Append(p.lastValue);
					Broacdcast(message);
					DebugService("broadcasting valyue change " + Const.valueOSC + "  " + p.memberId + "  " + p.baseName + "  " + p.lastValue);
				}
				else
				{
					OSCMessage message = new OSCMessage(Const.stringvalueOSC);
					message.Append(p.memberId);
					message.Append(p.lastStringValue);
					Broacdcast(message);
					DebugService("broadcasting string valyu change " + Const.valueOSC + "  " + p.memberId + "  " + p.baseName + "  " + p.lastValue);
				}
			}
		}
	}
	void Update()
	{
		UpdateQueues();
		if (Time.time > nextUpdateTime)
		{
			nextUpdateTime = Time.time + minimalUpdateTime;
			UpdateProxies();
		}

	}
}
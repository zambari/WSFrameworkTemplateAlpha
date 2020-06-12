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

	protected override void OnOSCMessage(OSCMessage message, WSServiceBehaviour beh)
	{
		string address = message.Address;
		if (address.StartsWith(Const.valueOSC))
		{
			ulong valueid = message.GetULong(0);
			float value = message.GetFloat(1);
			var proxy = ValueProxy.GetProxyFromDict(valueid);
			if (proxy == null)
			{
				Debug.Log("no proxy");
			}
			else
			{
				proxy.SetFloat(value);
			}
			DebugService("is value frame " + valueid.ToColorfulString() + " " + value);
		}

	}
	float minimalUpdateTime = 0.2f;
	float nextUpdateTime;
	void UpdateProxies()
	{
		foreach (var p in ValueProxy.activeProxies)
		{
			if (p.HasValueChanged())
			{
				
			}

		}
	}
	void Update()
	{
		if (Time.time > nextUpdateTime)
		{
			nextUpdateTime = Time.time + minimalUpdateTime;
			UpdateProxies();
		}

	}
}
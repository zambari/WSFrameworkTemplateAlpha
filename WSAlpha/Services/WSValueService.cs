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
			int memberId = message.GetInt(0);
			var proxy = ValueProxy.GetProxyFromDict(memberId);
			// tu slownik

			float value = message.GetFloat(1);
			DebugService("valueservice full adderss is " + address + " typetag " + message.typeTag + " vali " + memberId.ToColorfulString());

			if (proxy == null)
			{
				Debug.Log("no proxy");
			}
			else
			{
				proxy.SetFloat(value);
			}
			DebugService("is value frame " + memberId.ToColorfulString() + " " + value);
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Z.Reflection;

public class ValueProxy
{
	MemberInstanceLink linkreference;
	object objectReference;
	public string name;
	object lastValue;
	public bool HasValueChanged()
	{
		object val = linkreference.GetObject(objectReference);
		if (lastValue != val)
		{
			Debug.Log(" value hasn changed " + name);
			return true;
		}
		else
		{
			Debug.Log(" value hasn't changed " + name);
			return false;
		}
	}

	public ValueProxy(MemberInstanceLink link, object obj)
	{
		linkreference = link;
		objectReference = obj;
	}
	public void SetFloat(float f)
	{
		Debug.Log("seting float " + f);
		linkreference.SetFloat(objectReference, f);
	}
	public float GetFloat()
	{
		return linkreference.GetFloat(objectReference);
	}
	public static Dictionary<ulong, ValueProxy> proxyDict;
	public static List<ValueProxy> activeProxies = new List<ValueProxy>();
	public static void RegisterProxy(ulong valueid, ValueProxy proxy)
	{
		if (proxyDict == null) proxyDict = new Dictionary<ulong, ValueProxy>();
		proxyDict.Add(valueid, proxy);
		if (!activeProxies.Contains(proxy)) activeProxies.Add(proxy);
		Debug.Log("registered proxy for " + valueid.ToColorfulString() + " proxy " + proxy.name);
	}
	public static ValueProxy GetProxyFromDict(ulong valueidy)
	{

		if (proxyDict == null) proxyDict = new Dictionary<ulong, ValueProxy>();
		ValueProxy value = null;
		if (proxyDict.TryGetValue(valueidy, out value))
		{

		}
		return value;
		// Debug.Log("registered proxy for "+valueid.ToColorfulString());
	}
}
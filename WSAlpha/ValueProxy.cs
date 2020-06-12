using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Z.Reflection;

public class ValueProxy
{

	
	static System.Int32 _lastindex;
	public static System.Int32 nextValueIndex { get { return _lastindex++; } }
	/// <summary>
	/// Compacts provided id and fills the freed bits with index it increases
	/// </summary>
	public static ulong MakeValueUnique(ulong id)
	{
		return id.Compact() | (ulong) (long) nextValueIndex;
	}
	MemberInstanceLink linkreference;
	object objectReference;
	public ValueProxy(MemberInstanceLink link,object obj)
	{
		linkreference = link;
		objectReference=obj;
	}
	public void SetFloat(  float f)
	{	
		Debug.Log("seting float " + f);
		linkreference.SetFloat(objectReference,f);
		
	}

	public static Dictionary<ulong, ValueProxy> proxyDict;
	public static void RegisterProxy(ulong valueid, ValueProxy proxy)
	{
		if (proxyDict == null) proxyDict = new Dictionary<ulong, ValueProxy>();
		proxyDict.Add(valueid, proxy);
		Debug.Log("registered proxy for " + valueid.ToColorfulString());
	}
	public static ValueProxy GetProxy(ulong valueidy)
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
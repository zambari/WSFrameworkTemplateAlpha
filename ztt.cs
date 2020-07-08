using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityOSC;
using Z;
public class ztt : MonoBehaviour
{

	// Use this for initialization
	public string osca = "heello";
	[ExposeMethodInEditor]
	void test()
	{
		var oscme = new OSCMessage(osca);
		oscme.Append("aaaa");
		oscme.Append(0.5f);
		//oscme.Append(5);
		oscme.Append("yoyoy");
		//	oscme.Append(0);
		zOSCMessage mymesg = new zOSCMessage(oscme.BinaryData);
		Debug.Log(oscme.BinaryData.ByteArrayToString());
		Debug.Log("mymesg " + mymesg.types.Count);
		foreach (var t in mymesg.types)
		{
			Debug.Log("type " + t);
		}
	}

}
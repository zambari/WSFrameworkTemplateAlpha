using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectIDSet : MonoBehaviour
{

	public string enterHexString = "[aabbccdd]";
	public ulong converted;
	private void OnValidate()
	{
		converted = enterHexString.FromFingerPrint();
	}

	[ExposeMethodInEditor]
	void Apply()
	{
		GetComponent<ObjectID>().OverrideIdentifier(converted);
	}
	private void Awake()
	{
		GameObject.DestroyImmediate(this);
	}
}
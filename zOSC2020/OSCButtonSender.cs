using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OSCButtonSender : MonoBehaviour
{
	public bool sendIntParameter;
	public bool sendFloatParameter;
	public int intParameter;
	public float floatParameter;

	public string oscAddress = "/test";

	void Start()
	{
		GetComponent<Button>().onClick.AddListener(Clicked);
	}
	void Clicked()
	{
		if (sendIntParameter)
		{
			zOSC.SendOSC(oscAddress, intParameter);
		}
		else
		if (sendFloatParameter)
		{
			zOSC.SendOSC(oscAddress, floatParameter);
		}
		else
		{
			zOSC.SendOSC(oscAddress);
		}

	}
}
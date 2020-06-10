using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityOSC;

public class zOSCTestingSetupRequester : MonoBehaviour
{

	// Use this for initialization
	public ISendOSCSelector selector;
	ISendOSC sender;
	public int portToRequest = 7012;
	public string listenerType = "test";
	private void OnValidate()
	{
		selector.OnValidate(this);
	}
	public void RequestSetup()
	{
		sender = selector.valueSource;
		if (sender == null)
		{
			Debug.Log("no sender");
		}
		else
		{
			string addr = zBench.GetIPAddress();
			OSCMessage message = new OSCMessage("/setup/" + listenerType);
			message.Append(addr);
			message.Append(portToRequest);
			sender.Send(message);
		}
		// OSCSenderSelector.
	}
}
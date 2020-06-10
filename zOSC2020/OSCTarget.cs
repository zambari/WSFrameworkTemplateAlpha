using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.UI;
using UnityOSC;

[System.Serializable]
public class OSCTarget
{
	public string targetAddr = "127.0.0.1";
	public int targetPort = 9988;
	public OSCClient client;
	public bool use=true;
	public bool isConnected { get { return client != null; } }
	public OSCTarget(string targetAddr, int targetPort)
	{
		this.targetAddr = targetAddr;
		this.targetPort = targetPort;
	}
	public void Close()
	{
		if (client != null) client.Close();
	}
	public bool Connect()
	{
		client = new OSCClient(IPAddress.Parse(targetAddr), targetPort);
		if (client == null)
		{
			Debug.Log("target failed connecting");
			return false;
		}
		else
		{
			return true;
		}
	}
	public override string ToString()
	{
		return targetAddr + ":" + targetPort;
	}
}
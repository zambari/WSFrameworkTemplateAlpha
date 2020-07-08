using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityOSC;

public class zOSC : zOSCListener, ISendOSC //, ITakeOSCTarget
{
	public zOSCSender oscSender; // = new zOSCSenderModule();
	// public zOSCSender sender;
	// protected override void  OnValidate() {
	// 	// masterPort
	// 	// oscSender.tar
	// 	base.OnValidate();
	// 	oscSender.OnValidate(this) ;

	// 	}
	// }
	static zOSC instance;
	void Awake()
	{
		if (instance == null || instance == this) instance = this;
		else
		{
			Debug.Log("there are more sender instances");
		}
	}
	// void Reset()
	// // {
	// // 	sender = GameObject.FindObjectOfType<zOSCSender>();
	// // 	if (sender == null) sender = gameObject.AddComponent<zOSCSender>();
	// // }
	// protected override void Start()
	// {
	// 	base.Start();
	// 	StartSenderKeepalive();
	// 	if (oscSender.senderModule.autoConect)
	// 		oscSender.senderModule.ConnectToTargets();
	// }

	// void StartSenderKeepalive()
	// {
	// 	if (oscSender.senderModule.autoConect)
	// 		StartCoroutine(oscSender.CheckerRoutine());
	// }
	public static void SendOSC(OSCMessage msg)
	{
		Debug.Log("sending "+msg.ToReadableString());
		instance.oscSender.Send(msg);
	}

	public static void SendOSC(string oscAddress)
	{
		OSCMessage msg = new OSCMessage(oscAddress);
		SendOSC(msg);
	}
	public static void SendOSC(string oscAddress, int parameter)
	{
		
		OSCMessage msg = new OSCMessage(oscAddress);
		msg.Append(parameter);
		SendOSC(msg);
	}
	public static void SendOSC(string oscAddress, float parameter)
	{
		Debug.Log("sending " + parameter);
		OSCMessage msg = new OSCMessage(oscAddress);
		msg.Append(parameter);
		SendOSC(msg);
	}
	public void Send(OSCMessage msg)
	{
		oscSender.Send(msg);
	}

	// public void AddTarget(string addr, int port, bool active)
	// {
	// 	oscSender.AddTarget(addr, port, active);
	// }

	// public void RemoveTarget(string addr, int port)
	// {
	// 	throw new System.NotImplementedException();
	// }
}
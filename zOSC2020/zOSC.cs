using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityOSC;

public class zOSC : zOSCListener, ISendOSC,ITakeOSCTarget
{
	public zOSCSenderModule oscSender = new zOSCSenderModule();
	// protected override void  OnValidate() {
	// 	// masterPort
	// 	// oscSender.tar
	// 	base.OnValidate();
	// 	oscSender.OnValidate(this) ;
			
	// 	}
	// }
	protected override void Start()
	{
		base.Start();
		StartSenderKeepalive();
		if (oscSender.autoConect)
			oscSender.ConnectToTargets();
	}

	void StartSenderKeepalive()
	{
		if (oscSender.autoConect)
			StartCoroutine(oscSender.CheckerRoutine());
	}
	
	public void Send(OSCMessage msg)
	{
		oscSender.Send(msg);
	}

    public void AddTarget(string addr, int port)
    {
        oscSender.AddTarget(addr,port);
    }

    public void RemoveTarget(string addr, int port)
    {
        throw new System.NotImplementedException();
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityOSC;
[System.Serializable]
public class TargetAdderSelector : GenericSelector<ITakeOSCTarget> {}
public class zOSCSetupServer : MonoBehaviour, IConsumeOSC
{

	public int testport = 9988;

	public string listenportfunny;

	// public zOSC OSC;
	[Header("where to request	")]
	public TargetAdderSelector targetSelector;

	string messagePrefix { get { return "/setup"; } }
	public void OnMessage(OSCMessage msg)
	{
		Debug.Log("setup recieving typetag " + msg.typeTag);
		if (!msg.Address.StartsWith(messagePrefix))
			return;
		if (msg.typeTag.StartsWith(",si"))
		{
			string adr = null;
			int index = 0;
			adr = msg.GetString(ref index);
			int port = msg.GetInt(ref index);

			if (targetSelector.valueSource != null)
				targetSelector.valueSource.AddTarget(adr, port,true);
			Debug.Log("^trying to ad target  " + adr + ":" + port);
		}

	}

	private void OnValidate()
	{
		listenportfunny = zOSCNameAlpha.PortToName(testport);
		targetSelector.OnValidate(this);
	}

	// Use this for initialization
	void Start()
	{
		IRecieveOSC reciever = GetComponentInParent<IRecieveOSC>();
		if (reciever != null) reciever.Register(this);
	}

}
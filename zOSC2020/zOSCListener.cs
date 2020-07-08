using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityOSC;

public class zOSCListener : MonoBehaviour, IRecieveOSC
{
	public zOSCListenerModule oscListener = new zOSCListenerModule();
	public string listenportfunny;
	public int listenportdecoded;
	protected List<IConsumeOSC> consumers = new List<IConsumeOSC>();
	public bool printOnRecieve { get { return oscListener.stats.printOnRecieve; } set { oscListener.stats.printOnRecieve = value; } }
	public void Register(IConsumeOSC consumer)
	{
		consumers.Add(consumer);
	}
	protected virtual void OnValidate()
	{
		oscListener.OnValidate(this);
		listenportdecoded = 0;
		for (int i = 0; i < listenportfunny.Length; i++)
		{
			listenportdecoded = listenportdecoded << 1;
			listenportdecoded += (int) listenportfunny[i];
		}
		// listenportfunny = zOSCNameAlpha.PortToName(oscListener.listenPort);
	}

	void BroadcastPacketToConsumers(OSCMessage message)
	{
		foreach (var oscConsumer in consumers)
			oscConsumer.OnMessage(message);
		if (oscListener.stats.printOnRecieve)
		{
			if (useFilter && message.Address.Contains(filterString))
			{

			}
			else
			{
				Debug.Log("OSC in: " + message.ToReadableString()); //+ " (consumer count " + consumers.Count + ")"
			}
		}
	}
	public void Unregister(IConsumeOSC consumer)
	{
		consumers.Remove(consumer);
	}
	public void Reopen()
	{
		if (oscListener.RestartLocalServer())
		{
			Debug.Log("Osc lisener started at " + oscListener.listenPort);
		}
	}
	public void SetPort(string portAsString)
	{
		int port;
		if (System.Int32.TryParse(portAsString, out port))
		{
			oscListener.listenPort = port;
			Debug.Log("set listen port");
		}
	}
	protected virtual void Start()
	{
		if (oscListener.autoStart)
		{
			Reopen();
		}
		oscListener.useCustomReciever = true;
		oscListener.onPacket -= BroadcastPacketToConsumers;
		oscListener.onPacket += BroadcastPacketToConsumers;
	}
	public string filterString = "position";
	bool _useFilter = true;
	public bool useFilter { get { return _useFilter; } set { _useFilter = value; } }
	// protected virtual void OnValidate()
	// {
	// 	if (name == "GameObject") name = "zOSCAdditionalListener";
	// 	oscListener.loggingInfo.name = name;
	// }
	void Update()
	{
		oscListener.Update();
	}
	void OnDestroy()
	{
		if (oscListener != null && oscListener.server != null)
			oscListener.server.Close();
	}

}
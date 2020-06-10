using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityOSC;

public interface ISendOSC : Z.UnityBaseInterface
{
	void Send(OSCMessage msg);
}
public interface ITakeOSCTarget : Z.UnityBaseInterface
{
	void AddTarget(string addr, int port);
	void RemoveTarget(string addr, int port);
}
public interface IConsumeOSC : Z.UnityBaseInterface
{
	void OnMessage(OSCMessage msg);

}

public interface IRecieveOSC : IOSCListen {}
public interface IOSCListen : Z.UnityBaseInterface
{
	void Register(IConsumeOSC msg);
	void Unregister(IConsumeOSC msg);
}
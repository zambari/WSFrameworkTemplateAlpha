using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;
using Z;
public class WSPingClient : WSOSCClient
{
    // 	protected override void OnMessageDequeue(MessageEventArgs message)
    // 	{
    // 		DebugClient(" pingclient recieved " + message.Data);
    // 	}
    protected override void OnConnected()
    {
        if (OnConnection != null) OnConnection.Invoke(true);
        if (OnDisconnection != null) OnDisconnection.Invoke(false);
        pingSender = StartCoroutine(PinSenderRoutine());
    }
    protected override void OnDisconnected()
    {
        if (pingSender != null) StopCoroutine(pingSender);
        if (OnConnection != null) OnConnection.Invoke(false);
        if (OnDisconnection != null) OnDisconnection.Invoke(true);
        OnPingResult.Invoke("n/c");
    }
    Coroutine pingSender;
    public int pingInterval = 1;

    [ReadOnly] public int sentCount;
    [ReadOnly] public int recievedCount;
    [ReadOnly] public int pendingCount;
    System.Diagnostics.Stopwatch pingStopWatch;
    [Header("Events")]
    public BoolEvent OnConnection;
    [Header("Inverted ConConnection event (returns true on disconnection)")]
    public BoolEvent OnDisconnection;
    [Header("will fill with ping= text")]
    public StringEvent OnPingResult;

    protected override void OnOSCMessage(OSCMessage message)
    {
        if (message.Address.Contains("pong"))
        {
            recievedCount++;
            pendingCount--;
            int payload = message.GetInt(0);
            if (payload == sentCount)
            {
                if (pingStopWatch != null)
                {
                    var millis = pingStopWatch.ElapsedMilliseconds;
                    if (OnPingResult != null)
                        OnPingResult.Invoke("Ping: " + millis + " ms");
                }
            }
            else
            {
                DebugClient("non matching ping (was expecting " + sentCount + " recieved " + payload + ")");
            }
        }
        else
        {
            DebugClient("message did not contain pong");
        }
    }
    public IEnumerator PinSenderRoutine()
    {
        while (true)
        {
            yield return null;
            yield return null;
            SendPing();
            yield return new WaitForSeconds(pingInterval);
            //	SendPing();
        }
    }

    [ExposeMethodInEditor]
    public void SendPing()
    {
        sentCount++;
        pendingCount++;
        OSCMessage message = new OSCMessage("/ping");
        message.Append(sentCount);
        Send(message);
        pingStopWatch = new System.Diagnostics.Stopwatch();
        pingStopWatch.Start();
    }

}
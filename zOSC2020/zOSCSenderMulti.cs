using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.UI;
using UnityOSC;
using Z;

[System.Serializable]
public class zOSCSenderModule : zOSCLoggable
{
    [Space]
    public bool autoConect = true;
    [Space]
    [Header("Targets")]
    public List<OSCTarget> targets = new List<OSCTarget>() { new OSCTarget("127.0.0.1", 7788) };
    public CommStats statsSumary = new CommStats();
    public CommStats statsTotal = new CommStats();
    int reconnectInterval = 7;
    public void ConnectToTargets()
    {
        for (int i = 0; i < targets.Count; i++)
        {
            var thisTarget = targets[i];
            if (thisTarget.use)
            {
                if (!thisTarget.isConnected)
                {
                    //   thisTarget.Close();
                    thisTarget.Connect();
                    if (thisTarget.isConnected)
                    {
                        if (loggingInfo.writeToConsole)

                            Log("Connecting to " + thisTarget + " " + (thisTarget.isConnected? "Ok": "Failed"));
                    }
                }
            }
        }
    }

    public IEnumerator CheckerRoutine()
    {
        while (true)
        {
            yield return null;
            for (int i = 0; i < targets.Count; i++)
            {
                yield return null;
                var thisTarget = targets[i];
                if (thisTarget.use)
                {
                    if (thisTarget.isConnected)
                        thisTarget.Close();
                    thisTarget.Connect();
                    if (thisTarget.isConnected)
                    {
                        Log("Connecting to " + thisTarget + " " + (thisTarget.isConnected? "Ok": "Failed"));
                    }
                }
            }
            yield return new WaitForSeconds(reconnectInterval);
        }
    }
    public void AddTarget(string addr, int portNr, bool instantConnect = true)
    {
        var thisTarget = new OSCTarget(addr, portNr);
        if (instantConnect)
        {
            thisTarget.Connect();
        }
        targets.Add(thisTarget);
        Log(thisTarget + " conntected " + thisTarget.isConnected);
    }
    public void Send(OSCMessage message)
    {
        for (int i = 0; i < targets.Count; i++)
        {
            if (targets[i].use)
            {
                if (targets[i].isConnected)
                {
                    targets[i].client.Send(message);
                    statsTotal.AddBytesSent( message.BinaryData.Length);
                    // statsTotal.TotalBytes +=; // stats
                    // statsTotal.TotalPackets++; // stats
                }
                else
                {
                    Log("Diconnected target" + targets[i]);
                }
            }

        }
        if (loggingInfo.writeToConsole)
            Log("->" + message.Address + " " + message.typeTag);
        // statsSumary.TotalBytes += message.BinaryData.Length; // stats
        // statsSumary.TotalPackets++; // stats
    }

}
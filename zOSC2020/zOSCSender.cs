using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.UI;
using UnityOSC;
using Z;
public class zOSCSender : MonoBehaviour, ISendOSC, ITakeOSCTarget
{
    public zOSCSenderModule senderModule = new zOSCSenderModule();
    // void Start()
    // {
    //     if (autoConect)
    //         StartCoroutine(CheckerRoutine());
    // }
    // IEnumerator CheckerRoutine()
    // {
    //     while (true)
    //     {
    //         yield return null;
    //         if (client == null)
    //             SetTarget(targetAddr, targetPort);
    //         yield return new WaitForSeconds(reconnectInterval);
    //     }
    // }
    public void SetTargetEnabled(int targetIndex, bool use)
    {
        if (targetIndex < 0 || targetIndex >= senderModule.targets.Count) return;
        senderModule.targets[targetIndex].use = use;
    }
    public void AddTarget(string addr, int portNr, bool use)
    {
        senderModule.AddTarget(addr, portNr, use);
        // if (client != null) client.Close();
        // targetAddr = addr;
        // targetPort = portNr;
        // client = new OSCClient(IPAddress.Parse(addr), portNr);
        // if (client == null)
        // {
        //     Debug.Log("zOSC_1 sender port open failed  : " + addr + " : " + portNr);
        //     return false;
        // } else 
        //     Debug.Log("zOSC_1 sender is now targetting : " + addr + " : " + portNr);

        // return true;
    }

    public void RemoveTarget(string addr, int port)
    {
        throw new System.NotImplementedException();
    }

    public void Send(OSCMessage message)
    {
        senderModule.Send(message);
     
    }
    void Awake()
    {
        senderModule.loggingInfo.name = name;
    }
    void Start()
    {
        //base.Start();
        StartSenderKeepalive();
        if (senderModule.autoConect)
            senderModule.ConnectToTargets();
    }

    void StartSenderKeepalive()
    {
        if (senderModule.autoConect)
            StartCoroutine(senderModule.CheckerRoutine());
    }
}
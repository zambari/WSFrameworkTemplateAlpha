using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.UI;
using UnityOSC;
using Z;
public class zOSCSender : MonoBehaviour, ISendOSC
{
public zOSCSenderModule senderModule=new zOSCSenderModule();
    [Header("Sender")]
    public string targetAddr = "127.0.0.1";
    public int targetPort = 9988;
    public CommStats stats = new CommStats();

    OSCClient client;
    public bool autoConect = true;
    int reconnectInterval = 5;
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
    public void AddTarget(string addr, int portNr)
    {
         senderModule.AddTarget(addr,portNr);
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
   

    public void Send(OSCMessage message)
    {
        senderModule.Send(message);
        // if (client != null)
        // {

        //     //   if (!detailedLog)
        //     Debug.Log(message.Address + " " + message.typeTag);
        //     //   else
        //     //  Debug.Log(message.Address + " " + message.typeTag+"    binary: "+message.BinaryData.ByteArrayToStringAsHex());
        //     client.Send(message);
        //     stats.TotalBytes += message.BinaryData.Length; // stats
        //     stats.TotalPackets++; // stats
        //     Debug.Log("sent msg " + message.Address);

        // }
        // else
        // {
        //     Debug.Log("client is null");
        // }

    }

}
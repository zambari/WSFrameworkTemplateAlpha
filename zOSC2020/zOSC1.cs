
//  https://github.com/zambari/zOSC_1.Unity
//  An extension of
//  UnityOSC -   Copyright (c) 2012 Jorge Garcia Martin
//  base classes slightly modified, some wrappers added by zambari // Stereoko.TV

using UnityEngine;
using System.Collections.Generic;
using System;
using System.Net;
using UnityOSC;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections;
using Z.OSC;


// v 1.3 position and rotation in one packe3t
// v 1.4 merged zambox branch with terma branch
// v.2.0 new api, lots of improvements

#if nOT
public partial class zOSC_1 : MonoBehaviour
{
    [Header("Reciever@localhost")]
    public int defaultRecievePort = 8899;
    [Header("Sender")]
    public string targetAddr = "127.0.0.1";
    public int targetPort = 9988;

    [Header("Loopback")]
    public bool localEcho = true;
    ServerLog localListener;
    int listenPort = 9988;
    List<ClientLog> OSCRecievers;
    static zOSC_1 _instance;
    public static bool started = false;
    [Header("Console logging")]
    public bool logToConsole = true;
    public bool logSends = true;
    public bool detailedLog = false;

    [Header("AutoDiscovery")]

    public bool broadcastPresence = false;
    public bool listenToBroadcasts = false;
    bool isSendingBroadcast;

    [Header("Stats - Messages")]
    [ReadOnly]
    public int TotalPacketsSent;
    [ReadOnly]
    public int TotalPacketsRecieved;
    [Header("Stats - Traffic")]
    [ReadOnly]
    public int TotalBytesSent;
    [ReadOnly]
    public int TotalBytesRecieved;
    static bool warningDisplated;
    bool recieverIsLocal;
    Queue<OSCPacket> recievePacketQueue;

    // List<AckRequest> sentAckRequests;
    public static Action OnOSCRecieve;
    public static Action OnOSCTransmit;
    public static Action OnOSCTransmitLocal;
    public static OSCPacket lastRecieved;
    public static OSCMessage lastSent;
    public static OSCRouter get;

    // public static OSCRouter replyRouter;
    // public List<string> bindAddresses;
    OSCClient client;
    // static bool isCurrentPacketFromLoopback;
    void OnValidate()
    {
        if (!logToConsole)
        {
            logSends = false;
            detailedLog = false;
        }
    }

    void Reset()
    {
        if (name.Contains("GameObject")) name = "zOSC_1";
    }


    /// <summary>
    /// Contains incoming packet parsing ruleset
    /// </summary>
    /// <param name="packet"></param>

    void ReactToPacket(OSCPacket packet)
    {
        lastRecieved = packet;
        if (packet == null)
        {
            Debug.LogError("null packet");
            return;
        }
        if (logToConsole)
        {
            if (!detailedLog)
                zOSC_1.LogReceived("incoming " + packet.Address + " " + packet.typeTag);
            else
                zOSC_1.LogReceived("incoming " + packet.Address + "   typetag   " + packet.typeTag + "  " + packet.BinaryData.ByteArrayToStringAsHex());
        }
        _instance.TotalBytesRecieved += packet.BinaryData.Length;
        _instance.TotalPacketsRecieved++;

        // LIST BIND REQUESTES BEGIN    
        //   instance.listBindAdresses(address);
        // LIST BIND REQUESTES END
        try
        {
            // if (!isCurrentPacketFromLoopback)
            if (OnOSCRecieve != null) OnOSCRecieve.Invoke();
            else
            {
                Debug.Log("external");
            }
            //if (OnOSCRecieveLo != null) OnOSCRecieve.Invoke();

        }
        catch (Exception e)
        {
            Debug.Log("Exception " + e.Message + " when trying to notify local listeners of new packet");
        }
        ///// DO NOT REMOVE
        //   if (ParseAcks(packet)) return;
        //   checkIfAckRequired(ref packet);

        bool anyReacted = false;

        int i = 0;

        if (packet.Address[packet.Address.Length - 1] == '?')
        {
            Debug.Log("stopping as there was a request");
            listBindAdresses(packet.Address.Substring(0, packet.Address.Length - 1));
            return;
        }
        if (routers.Count == 0) Debug.Log("no routers");
        while (i < routers.Count)
        {
            if (packet.Address.StartsWith(routers[i].baseAddress))
            {
                // Debug.Log("potential");
                try
                {
                    if (routers[i].ParsePacket(packet))
                    {
                        //  Debug.Log("router [" + routers[i].baseAddress + "] reacted to " + packet.Address);
                        anyReacted = true;
                    }
                    else
                        Debug.Log("no router reacted to " + packet.Address);
                }
                catch (Exception e)
                {
                    Debug.Log("Exception while parsing router " + i + " [" + routers[i].baseAddress + "]  packet " + packet.Address + " triggered exception " + e.Message);

                }
            }
            i++;
        }
        if (!anyReacted) Log("Packet with no listeners " + packet.Address + " types: " + packet.typeTag);
    }


    // public void SetTarget(string addrr, int port)
    // {
    //     if (port != 0)
    //         targetPort = port;
    //     SetTarget(addrr, targetPort);
    // }


    public static bool SetTarget(string addr, int portNr)
    {
        if (_instance.client != null) _instance.client.Close();
        if (addr == "127.0.0.1" && portNr == instance.listenPort)
            _instance.recieverIsLocal = true;
        else
            _instance.recieverIsLocal = false;

        if (_instance.logToConsole)
            Debug.Log("zOSC_1 target : " + addr + " : " + portNr);

        _instance.targetAddr = addr;
        _instance.targetPort = portNr;
        _instance.client = new OSCClient(IPAddress.Parse(addr), portNr);
        if (_instance.client == null)
        {
            Log("OSC port open failed  : " + addr + " : " + portNr);
            return false;
        }
        return true;

    }


    public static bool SetLocalPort(int port)
    {
        instance.listenPort = port;
        instance.defaultRecievePort = port;
        return instance.RestartLocalServer();
    }

    IEnumerator Start()
    {
        //  Bind(this, OnBeaconDetection, "/beacon");
        //  Bind(this, SetTarget, "/target");

        SetLocalPort(defaultRecievePort);
        if (!string.IsNullOrEmpty(targetAddr))
        {
            if (broadcastPresence) PreparePresenceBroadcast(targetAddr);
            SetTarget(targetAddr, targetPort);
        }

        yield return new WaitForSeconds(0.1f);
        isSendingBroadcast = false;
        if (broadcastPresence)
            StartCoroutine(SendPings());

    }

    void Update()
    {
        if (recievePacketQueue != null)
            lock (recievePacketQueue)
            {
                while (recievePacketQueue.Count > 0)
                    ReactToPacket(recievePacketQueue.Dequeue());
            }

    }
    void Awake()
    {

        if (GetComponent<OSCHandler>() == null) gameObject.AddComponent<OSCHandler>();
        pendingAcks = new List<int>();
        _instance = this;
        if (_mainRouter != null)
        {
            Debug.Log("discarging exisitng router");
        }
        _mainRouter = new OSCRouter("");
        AddRouter(_mainRouter);

        //replyRouter = new OSCRouter("/");
        recievePacketQueue = new Queue<OSCPacket>();
    }


    void listBindAdresses(string addressSoFar)
    {
        List<string> availableAddresses = new List<string>();

        for (int i = 0; i < routers.Count; i++)
            routers[i].listBindAdresses(addressSoFar, ref availableAddresses);

        OSCMessage m = new OSCMessage(returnAddress + addressSoFar);
        foreach (string s in availableAddresses)
            m.Append(s);
        BroadcastOSC(m);


    }


}


#endif
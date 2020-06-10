using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityOSC;
using Z.OSC;

[System.Serializable]
public class zOSCListenerModule : zOSCLoggable
{
    [Space]
    public bool autoStart = true;

    List<OSCRouter> routers;
    Queue<OSCPacket> recievePacketQueue = new Queue<OSCPacket>();
    [Space]
    public int listenPort = 9988;
    public CommStats stats = new CommStats();
    List<ClientLog> OSCRecievers;
    public OSCServer server;
    OSCRouter _mainRouter = new OSCRouter("");
    [HideInInspector]
    public bool useCustomReciever;
    public System.Action<OSCMessage> onPacket;
    void ReactToPacket(OSCPacket packet)
    {
        // lastRecieved = packet;
        if (packet == null)
        {
            Debug.LogError("null packet");
            return;
        }
        // if (!detailedLog)
        Log(" ->>incoming " + packet.Address + " " + packet.typeTag);
        // else
        // zOSC_1.LogReceived("incoming " + packet.Address + "   typetag   " + packet.typeTag + "  " + packet.BinaryData.ByteArrayToStringAsHex());
        stats.AddBytesSent( packet.BinaryData.Length);

        // LIST BIND REQUESTES BEGIN    
        //   instance.listBindAdresses(address);
        // LIST BIND REQUESTES END

        ///// DO NOT REMOVE
        //   if (ParseAcks(packet)) return;
        //   checkIfAckRequired(ref packet);

        bool anyReacted = false;

        int i = 0;

        if (packet.Address[packet.Address.Length - 1] == '?')
        {
            Log("stopping as there was a request");
            // listBindAdresses(packet.Address.Substring(0, packet.Address.Length - 1));
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
                        Log("no router reacted to " + packet.Address);
                }
                catch (Exception e)
                {
                    Log("Exception while parsing router " + i + " [" + routers[i].baseAddress + "]  packet " + packet.Address + " triggered exception " + e.Message);

                }
            }
            i++;
        }
        if (!anyReacted) Log("Packet with no listeners " + packet.Address + " types: " + packet.typeTag);
    }
    public void AddRouter(OSCRouter router)
    {
        if (routers == null) routers = new List<OSCRouter>();
        routers.Add(router);
    }
    public virtual void OnValidate(MonoBehaviour source = null)
    {

    }
    public void Awake()
    {
        // if (GetComponent<OSCHandler>() == null) gameObject.AddComponent<OSCHandler>();
        _mainRouter = new OSCRouter("");
        AddRouter(_mainRouter);
        //replyRouter = new OSCRouter("/");
    }

    public bool RestartLocalServer()
    {
        if (server != null)
        {
            server.Close();
            Log("closing server");
        }
        server = new OSCServer(listenPort);
        server.PacketReceivedEvent += (server, packet) =>
        {
            lock(recievePacketQueue)
            {
                recievePacketQueue.Enqueue(packet);
            }
        };
        return true;
    }

    public void Update()
    {
        if (recievePacketQueue != null)
        {
            lock(recievePacketQueue)
            {
                while (recievePacketQueue.Count > 0)
                {
                    var thispacket = recievePacketQueue.Dequeue();
                    if (useCustomReciever && onPacket != null)
                    {
                        onPacket(thispacket as OSCMessage);
                    }
                    else
                    {
                        ReactToPacket(thispacket);
                    }
                }
            }
        }

    }
}
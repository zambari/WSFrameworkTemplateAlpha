// using System;
// using System.Collections;
// using System.Collections.Generic;
// using System.Net;
// using UnityEngine;
// using UnityEngine.UI;
// using UnityOSC;
// using Z.OSC;

// namespace Z.OSC
// {
// #pragma warning disable 649
//     class AckRequest
//     {
//         public OSCPacket message;
//         public float time;
//         public int requested;

//     }
// }
// public partial class zOSC_1 : MonoBehaviour
// {

// #pragma warning disable 414
//     List<int> pendingAcks;
// #pragma warning restore 414


//     public const string returnAddress = "/_?";
//     public const string query = "?";
//     private string GetIP()
//     {
//         string strHostName = "";
//         strHostName = System.Net.Dns.GetHostName();

//         IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(strHostName);

//         IPAddress[] addr = ipEntry.AddressList;

//         return addr[addr.Length - 1].ToString();

//     }
//     private string[] GetIPs()
//     {
//         string strHostName = "";
//         strHostName = System.Net.Dns.GetHostName();

//         IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(strHostName);

//         IPAddress[] addr = ipEntry.AddressList;
//         string[] adresses = new string[addr.Length];
//         for (int i = 0; i < addr.Length; i++)
//             adresses[i] = addr[i].ToString();
//         return adresses;

//     }
// #pragma warning restore 649
//     void OnBeaconDetection(string address)
//     {
//         if (listenToBroadcasts && !isSendingBroadcast)
//         {
//             Debug.Log(" recieved taarget change " + address);
//             //            SetTarget(address);
//         }
//     }

//     void PreparePresenceBroadcast(string targetAddr)
//     {
//         isSendingBroadcast = true;
//         string savedTarget = targetAddr;
//         string[] myIPs = GetIPs();
//         foreach (string addr in myIPs)
//         {


//             string[] split = addr.Split('.');
//             if (split.Length == 4)
//             {

//                 //           string broadcastAddress = split[0] + "." + split[1] + "." + split[2] + ".255";
//                 //          Debug.Log("broadcasting presence " + broadcastAddress);
//                 //          SetTarget(broadcastAddress, targetPort);
//                 //          BroadcastOSC("/beacon", addr);
//             }
//             //  else Debug.Log("invalid broadcas");

//         }
//         //  SetTarget(targetAddr, targetPort);
//         targetAddr = savedTarget;
//     }



//     bool RestartLocalServer()
//     {
//         OSCHandler.Instance.closeAllListeners();
//         if (OSCHandler.Instance.Servers.ContainsKey("LocalHost"))
//             OSCHandler.Instance.Servers.Remove("LocalHost");
//         try
//         {
//             localListener = OSCHandler.Instance.CreateServer("LocalHost", listenPort);
//             localListener.server.PacketReceivedEvent += newOScPacket;

//             started = true;
//             //    Debug.Log("started listenning at port " + listenPort);
//         }
//         catch (Exception e)
//         {
//             Debug.Log("local port " + listenPort + " failed " + e.Message);
//             return false;
//         }

//         if (OSCHandler.Instance.Servers.ContainsKey("LocalHost"))
//         {
//             //    if (LogRecieve) Log("localhost open running on port : " + OSCHandler.Instance.Servers["LocalHost"].server.LocalPort);
//             if (logToConsole) Debug.Log("zOSC_1 localhost open running on port : " + OSCHandler.Instance.Servers["LocalHost"].server.LocalPort);
//         }
//         return true;
//     }

//     void newOScPacket(OSCServer s, OSCPacket packet)
//     {
//         lock (recievePacketQueue)
//         {
//             recievePacketQueue.Enqueue(packet);
//         }
//     }
//     public static string printableBlob(byte[] b, int length, int start = 0)
//     {

//         string s = " ->: ";
//         if (length > b.Length) length = b.Length;
//         for (int i = 0; i < b.Length; i++)
//             s += "[" + (b[i] > 65 ? ((char)b[i]).ToString() : " ") + "] ";
//         return s;

//     }

//     public static int localPort
//     {
//         get { return instance.listenPort; }
//     }


//     public static string makeQuery(string inp)
//     {
//         return inp + query;
//     }
//     bool ParseAcks(OSCPacket packet)
//     {
//         if (packet.Address.Equals("/ack"))
//         {
//             string typeTag = packet.typeTag;
//             // typetag should be iiii, and we
//             for (int i = 1; i < typeTag.Length; i++)
//             {
//                 if (typeTag[i] != 'i')
//                     Debug.Log(" strange ack packet");
//                 else
//                 {
//                     try
//                     {
//                         int tt = Int32.Parse(packet.Data[i - 1].ToString());
//                         if (sentAckRequests.ContainsKey(tt))
//                         {
//                             sentAckRequests.Remove(tt);
//                             Debug.Log("removed ack " + tt);
//                         }

//                     }
//                     catch (Exception e) { Debug.Log("Error ack at index " + i + " " + e.Message); }
//                 }
//             }
//             return true;
//         }
//         else return false;
//     }

//     public static zOSC_1 instance
//     {
//         get
//         {
//             if (_instance == null)
//             {
//                 //                throw new System.InvalidOperationException("zOSC_1 instance not present on scene, or its AWAKE has not been run yet");
//             }
//             return _instance;
//         }
//     }

//     // public static void reportUnhandled(string msg)
//     // {
//     //     Debug.Log(" unhadnled " + msg);
//     // }
//     bool CheckIfAckRequired(ref OSCPacket packet)
//     {
//         string typeTag = packet.typeTag;
//         if (typeTag[typeTag.Length - 1] == 'i')
//         {
//             Debug.Log("ack request ");
//             return true;
//         }
//         typeTag = packet.typeTag.Substring(0, typeTag.Length - 1);
//         try
//         {
//             int ackRequest = Int32.Parse(packet.Data[typeTag.Length - 1].ToString());
//             Debug.Log("ack nr " + ackRequest);
//             OSCMessage ack = new OSCMessage("/ack");
//             ack.Append(ackRequest);
//             Debug.Log("sent back ack " + ackRequest);
//             BroadcastOSC(ack);
//         }
//         catch (Exception e) { Debug.Log("Error ack " + e.Message); }
//         return false;

//     }
//     public void AddRouter(OSCRouter router)
//     {
//         if (routers == null) routers = new List<OSCRouter>();
//         routers.Add(router);
//     }

//     /* */

//     // public void SetLogSends(bool b)
//     // {
//     //     LogSends = b;
//     //     Log("Displaying sent messages :" + b);
//     // }

//     // public void SetLogRecieve(bool b)
//     // {
//     //     LogRecieve = b;
//     //     Log("Displaying recieved messages :" + b);
//     // }

//     /*

//         public static void request(string address, Action<float> parseReply)
//         {
//             //   if (String.IsNullOrEmpty(address) || address[0] != '/') { Debug.LogWarning("please start OSC addresses with /"); return; }
//             replyRouter.bind(null, parseReply,address );
//             BroadcastOSC("/get" + address);
//         }
//         public static void request(string address, Action<string[]> parseReply)
//         {
//             if (String.IsNullOrEmpty(address) || address[0] != '/') { Debug.LogWarning("please start OSC addresses with /"); return; }
//             replyRouter.bind(null, parseReply,address + returnAddress);
//             instance.listBindAdresses(address);
//             //BroadcastOSC(address+query);

//         }

//         public static void bindReply(MonoBehaviour source, Action<string[]> parseReply, string address)
//         {
//             if (String.IsNullOrEmpty(address) || address[0] != '/') { Debug.LogWarning("please start OSC addresses with /"); return; }
//             replyRouter.bind(source, parseReply,returnAddress+address);

//             Debug.Log("reply bind "+"returnAddress+address");

//             instance.listBindAdresses(address);
//             BroadcastOSC(address);

//         }
//     */



//     /*

//         public static void request(string address, Action<float[]> parseReply)
//         {
//             if (String.IsNullOrEmpty(address) || address[0] != '/') { Debug.LogWarning("please start OSC addresses with /"); return; }
//             replyRouter.bind(this,address, parseReply);
//             BroadcastOSC("/get" + address);
//         }

//         public static void request(string address, Action<byte[]> parseReply)
//         {
//             if (String.IsNullOrEmpty(address) || address[0] != '/') { Debug.LogWarning("please start OSC addresses with /"); return; }
//             replyRouter.bind(this,address, parseReply);
//             BroadcastOSC("/get" + address);
//         }


//         public static void request(string address, Action<List<object>> parseReply)
//         {
//             if (String.IsNullOrEmpty(address) || address[0] != '/') { Debug.LogWarning("please start OSC addresses with /"); return; }
//             replyRouter.bind(this,address, parseReply);
//             BroadcastOSC("/get" + address);
//         } 

//       */
//     /*
//         string[] listOSCCommands()
//         {
//             return bindAddresses.ToArray();
//         } */

//     public static void console(string s)
//     {

//         BroadcastOSC("/console/status", s);
//     }
//     public static bool logDetails { get { return _instance.detailedLog; } }

//     public static void Log(string s)
//     {
//         if (_instance != null)
//             Debug.Log(smallBegin + "zOSC_1: " + s + smallEnd);
//     }

//     public static void LogSend(string s)
//     {
//         if (_instance != null)
//             Debug.Log(smallBegin + "zOSC_1: " + sentString + s + smallEnd);
//     }

//     public static void LogReceived(string s)
//     {
//         if (_instance != null)
//             Debug.Log(smallBegin + "zOSC_1: " + receivedString + s + smallEnd);
//     }
//     public static void Log(string s, GameObject g)
//     {
//         if (_instance != null && _instance.detailedLog)

//             Debug.Log(smallBegin + "zOSC_1: " + s + smallEnd, g);
//     }

//     static string smallBegin { get { return "<size=8>"; } }
//     static string smallEnd { get { return "</size>"; } }
//     public static string sentString { get { return "<color=green>Sent:</color>"; } }
//     public static string receivedString { get { return "<color=#44FF44>Received:</color>"; } }
//     IEnumerator SendPings()
//     {
//         while (true)
//         {
//             yield return new WaitForSeconds(5);
//             if (broadcastPresence)
//                 BroadcastOSC("/ping", Time.time);
//         }
//     }
//     static Dictionary<int, AckRequest> sentAckRequests;
//     static List<OSCRouter> routers;

//     public static OSCRouter mainRouter
//     {
//         get
//         {
//             if (_mainRouter == null)
//             {
//                 if (Application.isPlaying)
//                 {
//                     var osc = GameObject.FindObjectOfType<zOSC_1>();
//                     if (osc != null)
//                     {
//                         Debug.Log("strainge");

//                     }
//                     else
//                     {
//                         GameObject game = new GameObject("zOSC_1");
//                         game.AddComponent<zOSC_1>();

//                     }
//                 }

//             }
//             return _mainRouter;
//         }
//     }
//     static OSCRouter _mainRouter;

// }
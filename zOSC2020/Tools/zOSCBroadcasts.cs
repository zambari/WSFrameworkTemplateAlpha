// //  https://github.com/zambari/zOSC_1.Unity

// using System;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityOSC;
// using Z.OSC;

// public partial class zOSC_1 : MonoBehaviour
// {

//     public static bool BroadcastOSC(OSCMessage message, bool requireAck = false)
//     {

//         zOSC_1.lastSent = message;
//         if (_instance == null && !warningDisplated) { warningDisplated = true; Debug.LogWarning("Please add zOSC_1 to your scene first"); return false; }
//         if (_instance.client != null)
//         {
//             if (_instance.logToConsole && _instance.logSends)
//             {
//                 if (!_instance.detailedLog)
//                 LogSend(message.Address + " " + message.typeTag);
//                 else
//               LogSend(message.Address + " " + message.typeTag+"    binary: "+message.BinaryData.ByteArrayToStringAsHex());
//             }

//             _instance.client.Send(message);
//             _instance.TotalBytesSent += message.BinaryData.Length; // stats
//             _instance.TotalPacketsSent++;                          // stats
//             if (OnOSCTransmit != null) OnOSCTransmit();
//         }
//         else Debug.Log("no client " + message.Address);
//         try
//         {
//             if (_instance.localEcho)
//             {
//              // if (OnOSCTransmitLocal != null && !realsend) 
//                  //OnOSCTransmitLocal();
//                  //isCurrentPacketFromLoopback=true;
//                 // if (!_instance.recieverIsLocal)
//                     _instance.ReactToPacket(message);                   // dogfeeding our message
//                 //     isCurrentPacketFromLoopback=false;
//             }
//             return true;
//         }
//         catch (Exception e)
//         {
//             Debug.Log("Error broadcasting OSC to local listenerd (not through network!) " + e.Message);
//             return false;
//         }

//     }


//     // public static void BroadcastOSC(string address, OSCPacketExtensions.PositionAndRotation pq)
//     // {
//     //     OSCMessage message = new OSCMessage(address);
//     //     Vector3 pos = pq.position;
//     //     Quaternion rot = pq.rotation;
//     //     message.Append(pos.x);
//     //     message.Append(pos.y);
//     //     message.Append(pos.z);

//     //     message.Append(rot.x);
//     //     message.Append(rot.y);
//     //     message.Append(rot.z);
//     //     message.Append(rot.w);
//     //     BroadcastOSC(message);
//     // }

//     public static void BroadcastOSC(string address)
//     {
//         BroadcastOSC(new OSCMessage(address));

//     }
//     public static void BroadcastOSC(string address, float v)
//     {

//         OSCMessage message = new OSCMessage(address);
//         message.Append(v);
//         BroadcastOSC(message);

//     }
//     public static void BroadcastOSC(string address, int v)
//     {

//         OSCMessage message = new OSCMessage(address);
//         message.Append(v);
//         BroadcastOSC(message);

//     }

//     public static void BroadcastOSC(string address, float[] v)
//     {
//         OSCMessage message = new OSCMessage(address);
//         for (int i = 0; i < v.Length; i++)
//             message.Append(v[i]);
//         BroadcastOSC(message);
//     }

//     public static void BroadcastOSC(string address, string s)
//     {

//         OSCMessage message = new OSCMessage(address);
//         message.Append(s);
//         BroadcastOSC(message);
//     }
//     public static void BroadcastOSC(string address, string[] s)
//     {
//         OSCMessage message = new OSCMessage(address);
//         for (int i = 0; i < s.Length; i++)
//             message.Append(s[i]);
//         BroadcastOSC(message);
//     }

//     public static void BroadcastOSC(string address, byte[] b)
//     {

//         OSCMessage message = new OSCMessage(address);
//         message.Append(b);
//         BroadcastOSC(message);
//     }

//     public static void BroadcastOSC(string address, List<object> o) //, string format //, bool requireAck = false
//     {
//         OSCMessage message = new OSCMessage(address);
//         byte[] b = o.Serialize();
//         message.Append(b);
//         BroadcastOSC(message);
//     }

//     public static void BroadcastOSC(string address, Quaternion q) //, bool requireAck = false
//     {
//         OSCMessage message = new OSCMessage(address);

//         message.Append(q.x);
//         message.Append(q.y);
//         message.Append(q.z);
//         message.Append(q.w);
//         BroadcastOSC(message);

//     }

//     public static void BroadcastOSC(string address, Vector3 v) //, bool requireAck = false
//     {

//         OSCMessage message = new OSCMessage(address);
//         message.Append(v.x);
//         message.Append(v.y);
//         message.Append(v.z);
//         BroadcastOSC(message); //, requireAck
//     }
//     /*    if (requireAck)
//            {
//                _instance.pendingAcks.Add(ackRequestCounter);
//                AckRequest ack = new AckRequest();
//                ack.message = message;
//                ack.time = Time.time;
//                ack.requested = ackRequestCounter;
//                if (sentAckRequests == null) sentAckRequests = new Dictionary<int, AckRequest>();
//                sentAckRequests.Add(ack.requested, ack);
//                message.Append(ackRequestCounter);
//                ackRequestCounter++;
//            }*/
// }
//
//	  UnityOSC - Open Sound Control interface for the Unity3d game engine
//
//	  Copyright (c) 2012 Jorge Garcia Martin
//
// 	  Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
// 	  documentation files (the "Software"), to deal in the Software without restriction, including without limitation
// 	  the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, 
// 	  and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// 	  The above copyright notice and this permission notice shall be included in all copies or substantial portions 
// 	  of the Software.
//
// 	  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// 	  TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// 	  THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// 	  CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
// 	  IN THE SOFTWARE.
//
/// 
///  
///  modified by Zambari/Stereoko.TV 2017

// .02 better handling of short messges

using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using UnityEngine;

// modded

using UnityOSC;

public sealed class OSCMessage : OSCPacket
{
    // warning, subsequent reads might give you array out of bounds error

    #region Constructors

    public OSCMessage(string address)
    {
        _typeTag = DEFAULT.ToString();
        this.Address = address;
        currentRreadIndex = 0;
    }

    public OSCMessage(string address, object msgvalue)
    {
        _typeTag = DEFAULT.ToString();
        this.Address = address;
        currentRreadIndex = 0;
        Append(msgvalue);

    }

    #endregion

    #region Member Variables
    private const char INTEGER = 'i';
    private const char FLOAT = 'f';
    private const char LONG = 'h';
    private const char ULONG = 'u';
    private const char DOUBLE = 'd';
    private const char STRING = 's';
    private const char BYTE = 'b';
    private const char DEFAULT = ',';

    //	private string _typeTag; // moved to packet in zOSC_1

    #endregion

    #region Properties
    #endregion

    #region Methods

    /// <summary>
    /// Specifies if the message is an OSC bundle.
    /// </summary>
    /// <returns>
    /// A <see cref="System.Boolean"/>
    /// </returns>
    override public bool IsBundle() { return false; }

    /// <summary>
    /// Packs the OSC message to binary data.
    /// </summary>
    override public void Pack()
    {
        List<byte> data = new List<byte>();

        data.AddRange(OSCPacket.PackValue(_address));
        OSCPacket.PadNull(data);

        data.AddRange(OSCPacket.PackValue(_typeTag));
        OSCPacket.PadNull(data);

        foreach (object value in _data)
        {
            data.AddRange(OSCPacket.PackValue(value));
            if (value is string || value is byte[])
            {
                OSCPacket.PadNull(data);
            }
        }

        this._binaryData = data.ToArray();
    }

    /// <summary>
    /// Unpacks an OSC message.
    /// </summary>
    /// <param name="data">
    /// A <see cref="System.Byte[]"/>
    /// </param>
    /// <param name="start">
    /// A <see cref="System.Int32"/>
    /// </param>
    /// <returns>
    /// A <see cref="OSCMessage"/>
    /// </returns>
    /// 
    public static OSCMessage Unpack(byte[] data, ref int start)
    {
        string address = OSCPacket.UnpackString(data, ref start);
        OSCMessage message = new OSCMessage(address);
        char[] tags = OSCPacket.UnpackString(data, ref start).ToCharArray();
        if (start == 8) message._typeTag = ",";
        foreach (char tag in tags)
        {
            object value;
            switch (tag)
            {
                case FLOAT:
                    value = OSCPacket.UnpackFloat(data, ref start);
                    break;

                case DEFAULT:
                    continue;

                case INTEGER:
                    value = OSCPacket.UnpackInt(data, ref start);
                    break;

                case LONG:
                    value = OSCPacket.UnpackLong(data, ref start);
                    break;
                case ULONG:
                    value = OSCPacket.UnpackULong(data, ref start);
                    break;
                case DOUBLE:
                    value = OSCPacket.UnpackDouble(data, ref start);
                    break;

                case STRING:
                    value = OSCPacket.UnpackString(data, ref start);
                    break;

                case BYTE:
                    value = OSCPacket.UnpackBytes(data, ref start);
                    break;

                default:
                    Console.WriteLine("Unknown tag: " + tag);
                    continue;
            }

            message.Append(value);
        }

        if (message.TimeStamp == 0)
        {
            message.TimeStamp = DateTime.Now.Ticks;
        }

        return message;
    }

    /// 
    // public static OSCMessage Unpack(byte[] data, ref int start)
    // {
    //     string address = OSCPacket.UnpackString(data, ref start);
    //     OSCMessage message = new OSCMessage(address);
    //     if (start >= data.Length || start == 8)
    //     {
    //         message._typeTag = ",";
    //     }
    //     else
    //     {
    //         //    message._typeTag  = OSCPacket.UnpackString(data, ref start);
    //         char[] tags = OSCPacket.UnpackString(data, ref start).ToCharArray();
    //         foreach (char tag in tags)
    //         {
    //             object value;
    //             switch (tag)
    //             {
    //                 case FLOAT:
    //                     value = OSCPacket.UnpackFloat(data, ref start);
    //                     break;

    //                 case DEFAULT:
    //                     continue;

    //                 case INTEGER:
    //                     value = OSCPacket.UnpackInt(data, ref start);
    //                     break;

    //                 case LONG:
    //                     value = OSCPacket.UnpackLong(data, ref start);
    //                     break;
    //                 case DOUBLE:
    //                     value = OSCPacket.UnpackDouble(data, ref start);
    //                     break;

    //                 case STRING:
    //                     value = OSCPacket.UnpackString(data, ref start);
    //                     break;

    //                 case BYTE:
    //                     value = OSCPacket.UnpackBytes(data, ref start);
    //                     break;

    //                 default:
    //                     Console.WriteLine("Unknown tag: " + tag);
    //                     continue;
    //             }
    //             message.Append(value);
    //         }
    //     }
    //     if (message.TimeStamp == 0)
    //     {
    //         message.TimeStamp = DateTime.Now.Ticks;
    //     }
    //     return message;
    // }

    char GetTypeTag<T>(T val)
    {
        //	Type type = val.GetType();
        if (val is System.Single) return FLOAT;
        if (val is System.String) return STRING;
        if (val is byte[]) return BYTE;
        if (val is System.Int32) return INTEGER;
        if (val is System.Int64) return LONG;
        if (val is System.UInt64) return ULONG;
        if (val is System.Double) return DOUBLE;
        return DEFAULT;
    }
    public override void Append<T>(T value)
    {
        _typeTag += GetTypeTag(value);
        _data.Add(value);
    }
    #endregion
}
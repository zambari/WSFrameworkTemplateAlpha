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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using UnityEngine;

namespace UnityOSC
{
    /// <summary>
    /// Models a OSC Packet over an OSC stream.
    /// </summary>
    abstract public class OSCPacket
    {

        public int currentRreadIndex;

        #region Member Variables
        protected List<object> _data;
        protected byte[] _binaryData;
        protected string _address;
        protected long _timeStamp;
        #endregion

        #region Properties

        protected string _typeTag;
        protected char[] _typeTagchar;

        public string typeTag
        {
            get { return _typeTag; }

        }

        public bool AssertTypeTag(int index, char expected)
        {

            if (index + 1 >= _typeTag.Length)
            {
                UnityEngine.Debug.Log(typeTag);
                UnityEngine.Debug.Log("typetag has len " + _typeTag.Length + " requyestng " + (index + 1));
                return false;
            }

            char actualTag = _typeTag[index + 1];
            if (expected != actualTag)
            {
                UnityEngine.Debug.Log("Typetag mismatch, is " + actualTag + " expecting " + expected);
                return false;
            }
            return true;
        }
        public string Address
        {
            get
            {
                return _address;
            }
            set
            {
                Trace.Assert(string.IsNullOrEmpty(_address) == false);
                _address = value;
            }
        }

        public List<object> Data
        {
            get
            {
                return _data;
            }
        }

        public byte[] BinaryData
        {
            get
            {
                Pack();
                return _binaryData;
            }
        }

        public long TimeStamp
        {
            get
            {
                return _timeStamp;
            }
            set
            {
                _timeStamp = value;
            }
        }
        #endregion

        #region Methods
        abstract public bool IsBundle();
        abstract public void Pack();
        abstract public void Append<T>(T msgvalue);

        /// <summary>
        /// OSC Packet initialization.
        /// </summary>
        public OSCPacket()
        {
            this._data = new List<object>();
            currentRreadIndex = 0;
        }

        /// <summary>
        /// Swap endianess given a data set.
        /// </summary>
        /// <param name="data">
        /// A <see cref="System.Byte[]"/>
        /// </param>
        /// <returns>
        /// A <see cref="System.Byte[]"/>
        /// </returns>
        protected static byte[] SwapEndian(byte[] data)
        {
            byte[] swapped = new byte[data.Length];
            for (int i = data.Length - 1, j = 0; i >= 0; i--, j++)
            {
                swapped[j] = data[i];
            }
            return swapped;
        }

        /// <summary>
        /// Packs a value in a byte stream. Accepted types: Int32, Int64, Single, Double, String and Byte[].
        /// </summary>
        /// <param name="value">
        /// A <see cref="T"/>
        /// </param>
        /// <returns>
        /// A <see cref="System.Byte[]"/>
        /// </returns>
        protected static byte[] PackValue<T>(T value)
        {
            object valueObject = value;

            //   Type type = value.GetType();
            byte[] data = null;

            if (value is float)
            {
                data = BitConverter.GetBytes((float) valueObject);
                if (BitConverter.IsLittleEndian) data = SwapEndian(data);
                return data;
            }
            // break;

            if (value is String)

                return Encoding.ASCII.GetBytes((string) valueObject);

            if (value is byte[])
            {
                byte[] valueData = ((byte[]) valueObject);
                List<byte> bytes = new List<byte>();
                bytes.AddRange(PackValue(valueData.Length));
                bytes.AddRange(valueData);
                data = bytes.ToArray();
                return data;
            }
            if (value is System.Int32)
            {
                data = BitConverter.GetBytes((int) valueObject);
                if (BitConverter.IsLittleEndian) data = SwapEndian(data);
                // UnityEngine.Debug.Log("data lent "+data.Length);
                if (data.Length < 4)
                {
                    UnityEngine.Debug.Log("int too short");
                    List<byte> blist = new List<byte>(data);
                    while (blist.Count < 4)
                    {
                        blist.Add(0);
                        UnityEngine.Debug.Log("adding 0");
                    }
                    data = blist.ToArray();
                }
                return data;
            }
            if (value is System.Int64)
            {
                data = BitConverter.GetBytes((long) valueObject);
                if (BitConverter.IsLittleEndian) data = SwapEndian(data);
                return data;
            }
            else
            if (value is System.UInt64)
            {
                data = BitConverter.GetBytes((ulong) valueObject);
                if (BitConverter.IsLittleEndian) data = SwapEndian(data);
                return data;
            }
            else
                throw new Exception("Unsupported data type." + value.ToString());
            //    return data;
        }

        /// <summary>
        /// Unpacks a value from a byte stream. Accepted types: Int32, Int64, Single, Double, String and Byte[].
        /// </summary>
        /// <param name="data">
        /// A <see cref="System.Byte[]"/>
        /// </param>
        /// <param name="start">
        /// A <see cref="System.Int32"/>
        /// </param>
        /// <returns>
        /// A <see cref="T"/>
        /// </returns>
        /// 
        static byte[] buff4 = new byte[4];
        static byte[] buff8 = new byte[8];
        protected static int UnpackInt(byte[] data, ref int start)
        {
            Array.Copy(data, start, buff4, 0, 4);
            start += 4;

            if (BitConverter.IsLittleEndian)
                buff4 = SwapEndian(buff4);
            return BitConverter.ToInt32(buff4, 0);
        }
        protected static float UnpackFloat(byte[] data, ref int start)
        {
            Array.Copy(data, start, buff4, 0, 4);
            start += 4;

            if (BitConverter.IsLittleEndian)
                buff4 = SwapEndian(buff4);
            return BitConverter.ToSingle(buff4, 0);
        }

        protected static double UnpackDouble(byte[] data, ref int start)
        {
            Array.Copy(data, start, buff8, 0, 8);
            start += 8;

            if (BitConverter.IsLittleEndian)
                buff8 = SwapEndian(buff8);
            return BitConverter.ToDouble(buff8, 0);
        }

        protected static ulong UnpackULong(byte[] data, ref int start)
        {
            Array.Copy(data, start, buff8, 0, 8);
            start += 8;
            if (BitConverter.IsLittleEndian)
                buff8 = SwapEndian(buff8);
            return BitConverter.ToUInt64(buff8, 0);
        }
        protected static long UnpackLong(byte[] data, ref int start)
        {
            Array.Copy(data, start, buff8, 0, 8);
            start += 8;
            if (BitConverter.IsLittleEndian)
                buff8 = SwapEndian(buff8);
            return BitConverter.ToInt64(buff8, 0);
        }
        protected static string UnpackString(byte[] data, ref int start)
        {
            int count = 0;
            for (int index = start; start + count < data.Length && data[index] != 0; index++) count++; //added short packet handlinf

            var r = Encoding.ASCII.GetString(data, start, count);
            start += count + 1;
            start = ((start + 3) / 4) * 4;
            return r;
        }
        protected static byte[] UnpackBytes(byte[] data, ref int start)
        {
            int length = UnpackInt(data, ref start);
            byte[] buffer = new byte[length];
            Array.Copy(data, start, buffer, 0, buffer.Length);
            start += buffer.Length;
            start = ((start + 3) / 4) * 4;
            return buffer;
        }

        // protected static T UnpackValue<T>(byte[] data, ref int start)
        // {
        //     object msgvalue; //msgvalue is casted and returned by the function
        //     Type type = typeof(T);
        //     byte[] buffername;
        //     if (type.Name == "String")
        //     {
        //         int count = 0;
        //         for (int index = start; data[index] != 0; index++) count++;

        //         msgvalue = Encoding.ASCII.GetString(data, start, count);
        //         start += count + 1;
        //         start = ((start + 3) / 4) * 4;
        //     }
        //     else if (type.Name == "Byte[]")
        //     {
        //         int length = UnpackValue<int>(data, ref start);
        //         byte[] buffer = new byte[length];
        //         Array.Copy(data, start, buffer, 0, buffer.Length);
        //         start += buffer.Length;
        //         start = ((start + 3) / 4) * 4;

        //         msgvalue = buffer;
        //     }
        //     else
        //     {
        //         switch (type.Name)
        //         {
        //             case "Int32":
        //             case "Single"://this also serves for float numbers
        //                 buffername = new byte[4];
        //                 break;

        //             case "Int64":
        //             case "Double":
        //                 buffername = new byte[8];
        //                 break;

        //             default:
        //                 throw new Exception("Unsupported data type.");
        //         }

        //         Array.Copy(data, start, buffername, 0, buffername.Length);
        //         start += buffername.Length;

        //         if (BitConverter.IsLittleEndian)
        //         {
        //             buffername = SwapEndian(buffername);
        //         }

        //         switch (type.Name)
        //         {
        //             case "Int32":
        //                 msgvalue = BitConverter.ToInt32(buffername, 0);
        //                 break;

        //             case "Int64":
        //                 msgvalue = BitConverter.ToInt64(buffername, 0);
        //                 break;

        //             case "Single":
        //                 msgvalue = BitConverter.ToSingle(buffername, 0);
        //                 break;

        //             case "Double":
        //                 msgvalue = BitConverter.ToDouble(buffername, 0);
        //                 break;

        //             default:
        //                 throw new Exception("Unsupported data type.");
        //         }
        //     }

        //     return (T)msgvalue;
        // }

        /// <summary>
        /// Unpacks an array of binary data.
        /// </summary>
        /// <param name="data">
        /// A <see cref="System.Byte[]"/>
        /// </param>
        /// <returns>
        /// A <see cref="OSCPacket"/>
        /// </returns>
        public static OSCPacket Unpack(byte[] data)
        {
            int start = 0;

            return Unpack(data, ref start, data.Length);
        }

        /// <summary>
        /// Unpacks an array of binary data given reference start and end pointers.
        /// </summary>
        /// <param name="data">
        /// A <see cref="System.Byte[]"/>
        /// </param>
        /// <param name="start">
        /// A <see cref="System.Int32"/>
        /// </param>
        /// <param name="end">
        /// A <see cref="System.Int32"/>
        /// </param>
        /// <returns>
        /// A <see cref="OSCPacket"/>
        /// </returns>
        public static OSCPacket Unpack(byte[] data, ref int start, int end)
        {
            if (data[start] == '#')
            {
                //   return OSCBundle.Unpack(data, ref start, end);
            }

            // else 
            return OSCMessage.Unpack(data, ref start);
        }

        /// <summary>
        /// Pads null a list of bytes.
        /// </summary>
        /// <param name="data">
        /// A <see cref="List<System.Byte>"/>
        /// </param>
        protected static void PadNull(List<byte> data)
        {
            byte nullvalue = 0;
            int pad = 4 - (data.Count % 4);
            for (int i = 0; i < pad; i++)
                data.Add(nullvalue);
        }

        #endregion
    }
}
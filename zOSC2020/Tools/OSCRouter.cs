///zambari codes unity
// STUCT CASTING VERION


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityOSC;
using System;
// v.02 vector2
// blob listeners
// v.02 postmerge
namespace Z.OSC
{
    public partial class OSCRouter
    {
        public string baseAddress;
        protected int baseAddressLen;
        protected bool isRoot;
        public bool oneShot;

        protected Dictionary<string, Binding> oscBinding;

        protected class Binding
        {
            public Type objectType;
            public string address;
            public object bindMethod;
            public MonoBehaviour requester;
            public Binding nextBind; // important - handles multiple bindings
        }


        #region bindOverloads

        #endregion bindOverloads

        #region useBind

        bool use(Action listener, OSCPacket packet)
        {
            if (listener == null) Debug.Log("no listener");
            else { listener.Invoke(); }
            return true;

        }
        // bool use(Action<byte[]> listener, OSCPacket packet)
        // {
        //     if (listener == null) Debug.Log("no listener");
        //     byte[] bytes;
        //     if (packet.GetBytes(out bytes))
        //         listener.Invoke(bytes);
        //     else { return false; }
        //     return true;

        // }
        // bool use(Action<string, byte[]> listener, OSCPacket packet)
        // {
        //     if (listener == null) Debug.Log("no listener");
        //     string s;
        //     byte[] bytes;
        //     if (packet.GetString(out s, 0) && packet.GetBytes(out bytes, 1))
        //     {
        //         Debug.Log("YOYOYO");
        //         listener.Invoke(s, bytes);
        //     }
        //     else { return false; }
        //     return true;

        // }
        // bool use(Action<float[]> listener, OSCPacket packet)
        // {
        //     Debug.Log("Trying bye binging");
        //     if (listener == null) Debug.Log("no listener");
        //     float[] vs;
        //     if (packet.getfloatArray(out vs))
        //     {
        //         listener.Invoke(vs);
        //         return true;
        //     }
        //     else
        //     {
        //         Debug.Log("param not matching");
        //         return true;
        //     }

        // }
        // bool use(Action<int> listener, OSCPacket packet)
        // {
        //     if (listener == null) Debug.Log("no listener");
        //     else
        //     {
        //         int i;
        //         if (packet.GetInt(out i))
        //             listener.Invoke(i);

        //     }
        //     return true;

        // }

        // bool use(Action<Vector3> listener, OSCPacket packet)
        // {
        //     if (listener == null) Debug.Log("no listener");
        //     else
        //     {
        //         Vector3 vector = Vector3.zero;
        //         if (packet.GetVector3(out vector))
        //             listener.Invoke(vector);
        //     }
        //     return true;
        // }

        // bool use(Action<Quaternion> listener, OSCPacket packet)
        // {
        //     if (listener == null) Debug.Log("no listener");
        //     else
        //     {
        //         Quaternion vector = Quaternion.identity;
        //         if (packet.GetQuaternion(out vector))
        //             listener.Invoke(vector);

        //     }
        //     return true;

        // }

        // bool use(Action<Vector2> listener, OSCPacket packet)
        // {
        //     if (listener == null) Debug.Log("no listener");
        //     else
        //     {
        //         Vector2 vector = Vector2.zero;
        //         if (packet.GetVector2(out vector))
        //             listener.Invoke(vector);

        //     }
        //     return true;

        // }
        // bool use(Action<float> listener, OSCPacket packet)
        // {
        //     float value;
        //     if (packet.GetFloat(out value))
        //     {
        //         listener.Invoke(value);
        //         return true;
        //     }
        //     return false;

        // }
        // bool use(Action<string> listener, OSCPacket packet)
        // {
        //     string value=packet.GetString()
        //     if ((out value))
        //     {
        //         listener.Invoke(value);
        //         return true;
        //     }
        //     return false;

        // }
        // bool use(Action<string[]> listener, OSCPacket packet)
        // {
        //     string[] value;
        //     if (packet.GetStringArray(out value))
        //     {
        //         listener.Invoke(value);
        //         return true;
        //     }
        //     return false;
        // }

        // bool use(Action<OSCPacketExtensions.PositionAndRotation> listener, OSCPacket packet)
        // {
        //     OSCPacketExtensions.PositionAndRotation value;
        //     if (packet.GetPositionAndRotation(out value))
        //     {
        //         listener.Invoke(value);
        //         return true;
        //     }
        //     return false;
        // }


        bool UseBinding(Binding listener, OSCPacket packet)
        {

            bool returned = false;

        //     Type savedType = listener.objectType;

        //     if (savedType == typeof(Action))
        //         returned = use(((Action)listener.bindMethod), packet);
        //     else
        //     if (savedType == typeof(Action<float>))
        //         returned = use(((Action<float>)listener.bindMethod), packet);
        //     else
        //     if (savedType == typeof(Action<int>))
        //         returned = use(((Action<int>)listener.bindMethod), packet);
        //     else
        //       if (savedType == typeof(Action<string>))
        //         returned = use((Action<string>)listener.bindMethod, packet);
        //     else
        //       if (savedType == typeof(Action<byte[]>))
        //         returned = use((Action<byte[]>)listener.bindMethod, packet);
        //     else
        //            if (savedType == typeof(Action<string, byte[]>))
        //         returned = use((Action<string, byte[]>)listener.bindMethod, packet);
        //     else
        //    if (savedType == typeof(Action<string[]>))
        //         returned = use((Action<string[]>)listener.bindMethod, packet);
        //     else
        //     if (savedType == typeof(Action<float[]>))
        //         returned = use((Action<float[]>)listener.bindMethod, packet);
        //     else
        //       if (savedType == typeof(Action<Vector3>))
        //         returned = use((Action<Vector3>)listener.bindMethod, packet);
        //     else
        //       if (savedType == typeof(Action<Vector2>))
        //         returned = use((Action<Vector2>)listener.bindMethod, packet);
        //     else
        //      if (savedType == typeof(Action<Quaternion>))
        //         returned = use((Action<Quaternion>)listener.bindMethod, packet);
        //     else
        //     if (savedType == typeof(Action<byte[]>))
        //         returned = use((Action<byte[]>)listener.bindMethod, packet);
        //     else
        //        if (savedType == typeof(Action<OSCPacketExtensions.PositionAndRotation>))
        //         returned = use((Action<OSCPacketExtensions.PositionAndRotation>)listener.bindMethod, packet);

        //     // else
        //     //  if (savedType == typeof(Action<Quaternion>))
        //     // {

        //     //     returned = use((Action<Quaternion>)listener.bindMethod, packet);
        //     // }
        //     // providers
        //     object provider = listener.bindMethod;

        //     if (!returned)
        //     {

        //         if (savedType == typeof(Func<string>))
        //         {
        //             var returnVal = ((Func<string>)provider)();
        //             zOSC_1.BroadcastOSC(zOSC_1.returnAddress + packet.Address, returnVal);
        //             returned = true;

        //         }
        //         else
        //         if (savedType == typeof(Func<float>))
        //         {
        //             var returnVal = ((Func<float>)provider)();
        //             Debug.Log("broadcasting return float " + returnVal);
        //             zOSC_1.BroadcastOSC(zOSC_1.returnAddress + packet.Address, returnVal);

        //             returned = true;
        //         }
        //         else
        //         if (savedType == typeof(Func<float[]>))
        //         {
        //             var returnVal = ((Func<float[]>)provider)();
        //             zOSC_1.BroadcastOSC(zOSC_1.returnAddress + packet.Address, returnVal);
        //             returned = true;
        //         }
        //         else
        //         if (savedType == typeof(Func<string[]>))
        //         {
        //             var returnVal = ((Func<string[]>)provider)();
        //             zOSC_1.BroadcastOSC(zOSC_1.returnAddress + packet.Address, returnVal);
        //             returned = true;
        //         }
        //     }

        //     if (oneShot) // typically this means this is a request, and before we reply we remove it from the bind list
        //     {
        //         oscBinding.Remove(listener.address);
        //     }
        //     else
        //     if (listener.nextBind != null)
        //     {
        //         // Debug.Log("going for reccuence");
        //         UseBinding(listener.nextBind, packet);
        //         // Debug.Log("aand we're back");
        //     }
            return returned;
        }

    public virtual bool ParsePacket(OSCPacket packet)
    {
        if (String.IsNullOrEmpty(packet.Address))
        {
            Debug.Log("null packet address"); return false;
        }
        //        Debug.Log(baseAddressLen + " " + packet.Address);
        try
        {
            string address = packet.Address.Substring(baseAddressLen);

            // for (int i = 0; i < oscPartialBinding.Count; i++)
            // {
            //     string thisString = oscPartialBinding[i].Key;
            //     if (address.StartsWith(thisString))
            //     {
            //         UseBinding(oscPartialBinding[i].Value, packet);
            //     }
            //     else Debug.Log("not matched");
            // }

            Binding listener;
            if (oscBinding.TryGetValue(address, out listener))
            {
                try
                {
                    return UseBinding(listener, packet);
                }
                catch (Exception e)
                {
                    Debug.LogError("Error using binding " + e.Message);
                    if (listener.requester != null)
                        Debug.LogError("Binding info: " + address + " Monobehaviour: " + listener.requester.GetType().ToString() + " on gameobject " + listener.requester.name + " " + listener.objectType.ToString() + " " + listener.bindMethod.GetType().ToString());
                }
            }

        }
        catch (Exception e) { Debug.LogError("invalid string " + e.Message); }
        return true;
    }

    public OSCRouter(string s)
    {
        baseAddress = s;
        baseAddressLen = baseAddress.Length;
        oscBinding = new Dictionary<string, Binding>();
//        oscPartialBinding = new List<KeyValuePair<string, Binding>>();
    }
    #endregion useBind

    public void listBindAdresses(string startStrting, ref List<string> list)
    {
        foreach (string s in oscBinding.Keys)
            if (!list.Contains(s))
                list.Add(s);
    }

    // bool checkIfFloatArray(string address, OSCPacket packet)
    // {
    //     string typeTag = packet.typeTag;
    //     if (typeTag.Length <= 1) return false;
    //     for (int i = 1; i < typeTag.Length; i++)
    //         if (typeTag[i] != 'f')
    //             return false;
    //         float[] floatArr = new float[typeTag.Length - 1];
    //         float nextFloat = 0;
    //         for (int i = 0; i < typeTag.Length - 1; i++)
    //         {
    //             if (Single.TryParse(packet.Data[i].ToString(), out nextFloat))
    //                 floatArr[i] = nextFloat;
    //             else
    //             {
    //                 Debug.Log("parsing float failed" + address + "   " + packet.Data[i].ToString());
    //                 return false;
    //             }
    //         }
        
    //     return true;
    //      }
    // }
    // protected virtual void reportUnBind<T>(string addr)
    // {
    //     //   zOSC_1.reportUnBind(addr);
    // }
    // protected virtual void reportBind<T>(string addr)
    // {
    //     //  zOSC_1.reportBind(addr);
    // }

    public virtual void Unbind(MonoBehaviour requester, string addr)
    {
        if (String.IsNullOrEmpty(addr)) return;
        if (oscBinding.ContainsKey(addr))
        {
            Binding thisBinding = oscBinding[addr];
            if (thisBinding.requester != requester)
                while (thisBinding.nextBind != null)
                {
                    thisBinding = thisBinding.nextBind;
                }
            oscBinding.Remove(addr);
        }

    }

    // protected bool stringWrong(string s)
    // {
    //     if (String.IsNullOrEmpty(s))
    //     { /*Debug.Log("t");*/ return true; }
    //     if (s[0] != '/' || (s.Length < 2))
    //     {
    //         Debug.Log("osc adresses should start with '/'");
    //         return true;
    //     }

    //     return false;
    // }
}

}
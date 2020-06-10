using System;
using UnityEngine;
using Z.OSC;

//  https://github.com/zambari/zOSC_1.Unity
#if not
public partial class zOSC_1 : MonoBehaviour
{



    public static void Bind(MonoBehaviour requester, string addr, Action listener)
    {
        mainRouter.bindGeneric(requester, addr, listener);
    }
    public static void Bind(MonoBehaviour requester, string addr, Action<float> listener)
    {
        mainRouter.bindGeneric(requester, addr, listener);
    }
    public static void BindInt(MonoBehaviour requester, string addr, Action<int> listener)
    {
        mainRouter.bindGeneric(requester, addr, listener);
    }
    public static void Bind(MonoBehaviour requester, string addr, Action<string, byte[]> listener)
    {
        mainRouter.bindGeneric(requester, addr, listener);
    }
    public static void Bind(MonoBehaviour requester, string addr, Action<string> listener)
    {
        mainRouter.bindGeneric(requester, addr, listener);
    }

    public static void Bind(MonoBehaviour requester, string addr, Action<byte[]> listener)
    {
        mainRouter.bindGeneric(requester, addr, listener);
    }
    public static void Bind(MonoBehaviour requester, string addr, Action<float[]> listener)
    {
        //  Debug.Log("binding float[]");
        mainRouter.bindGeneric(requester, addr, listener);
    }

    public static void Bind(MonoBehaviour requester, string addr, Action<string[]> listener)
    {
        mainRouter.bindGeneric(requester, addr, listener);
    }

    // public static void Bind(MonoBehaviour requester, string addr, Action<List<object>> listener)
    // {
    //     mainRouter.bindGeneric(requester, addr, listener);
    // }
    public static void BindVector3(MonoBehaviour requester, string addr, Action<Vector3> listener)
    {
        mainRouter.bindGeneric(requester, addr, listener);
    }
    public static void Bind(MonoBehaviour requester, string addr, Action<Vector2> listener)
    {
        mainRouter.bindGeneric(requester, addr, listener);
    }

    // public static void Bind(MonoBehaviour requester, string addr, Action<OSCPacketExtensions.PositionAndRotation> listener)
    // {
    //     mainRouter.bindGeneric(requester, addr, listener);
    // }
    /*    public static void Bind(MonoBehaviour requester, string addr, Action<Vector2> listener)
    {
        mainRouter.bindGeneric(requester, addr, listener);
    }*/
    public static void Bind(MonoBehaviour requester, string addr, Action<Quaternion> listener)
    {
        mainRouter.bindGeneric(requester, addr, listener);
    }

    // public static void Bind(MonoBehaviour requester, string addr, Action<Quaternion> listener)
    // {
    //     mainRouter.bindGeneric(requester, addr, listener);
    // }
    public static void Unbind(string addr)
    {
        mainRouter.Unbind(null, addr);
    }

    [Obsolete("Please use new parameter order: Monobehaviour, address, method")]
    public static void Bind(MonoBehaviour requester, object o, string addr)
    {
        throw new System.NotImplementedException("Parameter order has changed since zOSC_1 1.1, pleas update your code");
    }
}
#endif
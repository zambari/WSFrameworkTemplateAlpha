using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityOSC;
using WebSocketSharp;
using WSFrameworkConst;
//zbr 2020

public class WSTransformClient : WSOSCClient
{
    public bool local;
    public bool set;
    public bool rotation;
    public bool position;
    public bool scale;
    public ulong id;
    public Transform localTransform;
    protected override void Start()
    {
        base.Start();

        var oid = localTransform.gameObject.AddComponent<ObjectID>();
        oid.OverrideIdentifier(id);
    }
    [Range(0,0.3f)]
    public   float minRequestTime = 0.1f;
    float lastREquestTime;
    protected override void Update()
    {
        base.Update();
        if (isConnected && Time.time > lastREquestTime + minRequestTime)
        {
            lastREquestTime = Time.time;
            MakeRequest();
        }
    }
    [ExposeMethodInEditor]
    void MakeRequest()
    {
        OSCMessage message = null;
        if (set)
        {
            message = localTransform.ReportState(local, position, rotation, scale);
            Send(message);

        }
        else
        {
            message = id.QueryState(local, position, rotation, scale);
        }
        Send(message);
        DebugClient(message.ToReadableString());
        // DebugClient(addres+" request sent " + message.ToReadableString());
    }
    protected override void OnOSCMessage(OSCMessage message)
    {
        string address = message.Address;
        ulong id = message.GetULong(0);
        Transform targettedTransform = id.FindTransform();
        if (targettedTransform != null)
        {
            // address = address.OSCFollowingSemgents();
            // bool local = false;
            // if (address.StartsWith(Const.local))
            // {
            //     local = true;
            // }
            // else
            // if (address.StartsWith(Const.global))
            // {
            //     local = false;
            // }
            // else
            // {
            //     DebugClient("invalide");
            // }
            // address = address.OSCFollowingSemgents();
            // if (address.StartsWith(Const.position))
            // {
            //     Vector3 position = new Vector3(message.GetFloat(1), message.GetFloat(2), message.GetFloat(3));
            //     if (local)
            //         targettedTransform.localPosition = position;
            //     else
            //         targettedTransform.position = position;
            // }
            // else
            // if (address.StartsWith(Const.rotation))
            // {
            //     Quaternion rotation = new Quaternion(message.GetFloat(1), message.GetFloat(2), message.GetFloat(3), message.GetFloat(4));
            //     if (local)
            //         targettedTransform.localRotation = rotation;
            //     else
            //         targettedTransform.rotation = rotation;
            // }
            // if (address.StartsWith(Const.positionRotation))
            // {
            //     Vector3 position = new Vector3(message.GetFloat(1), message.GetFloat(2), message.GetFloat(3));
            //     Quaternion rotation = new Quaternion(message.GetFloat(4), message.GetFloat(5), message.GetFloat(6), message.GetFloat(7));
            //     if (local)
            //     {
            //         targettedTransform.localPosition = position;
            //         targettedTransform.localRotation = rotation;
            //     }
            //     else
            //     {
            //         targettedTransform.position = position;
            //         targettedTransform.rotation = rotation;
            //     }
            // }
            // if (address.StartsWith(Const.posRotScale))
            // {
            //     Vector3 position = new Vector3(message.GetFloat(1), message.GetFloat(2), message.GetFloat(3));
            //     Quaternion rotation = new Quaternion(message.GetFloat(4), message.GetFloat(5), message.GetFloat(6), message.GetFloat(7));
            //     Vector3 scale = new Vector3(message.GetFloat(8), message.GetFloat(9), message.GetFloat(10));
            //     targettedTransform.localPosition = position;
            //     targettedTransform.localRotation = rotation;
            //     targettedTransform.localScale = scale;
            //     targettedTransform.position = position;
            //     targettedTransform.rotation = rotation;
            targettedTransform.ApplyState(message);
            DebugClient("set  params " + message.ToReadableString());
        }
        else
        {
            DebugClient("transform not found " + id);
        }

    }


}
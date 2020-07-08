using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityOSC;
using WSFrameworkConst;
using Z;
using Z.Reflection;

public class WSTransformService : WSOSCService
{

    // public List<ValueProxy> activeProxies = new List<ValueProxy>();
    float minimalUpdateTime = 0.2f;
    float nextUpdateTime;
    protected override void OnOSCMessage(OSCMessage message, WSServiceBehaviour beh)
    {
        string address = message.Address;
        ulong id = message.GetULong(0);
        Transform targettedTransform = id.FindTransform();
        if (targettedTransform != null)
        {
            if (address.StartsWith(Const.get))
            {

                address = address.OSCFollowingSemgents();
                bool local = false;
                if (address.StartsWith(Const.local))
                {
                    local = true;
					DebugService("local");
                }
                else
                if (address.StartsWith(Const.global))
                {
                    local = false;
					DebugService("global");
                }
                else
                {
                    DebugService("invalide");
                }
                address = address.OSCFollowingSemgents();
                bool rotation = false;
                // OSCMessage response = new OSCMessage("");
                // response.Append(id);
                bool position = true;
                if (address.StartsWith(Const.position))
                {
                    rotation = false;
                }
                else
                 if (address.StartsWith(Const.rotation))
                {
                    position = false;
                    rotation = true;

                }
                if (address.StartsWith(Const.positionRotation))
                {
                    position = true;
                    rotation = true;
                }
                bool scale = false;
                if (address.StartsWith(Const.posRotScale))
                {
                    scale = true;
					position = true;
                    rotation = true;
                }
                OSCMessage response = targettedTransform.ReportState(local, position, rotation, scale);
				beh.Send(response);
                // DebugService("sent response " + response.ToReadableString());
            }
            else
            if (address.StartsWith(Const.set))
            {
                targettedTransform.ApplyState(message);
                DebugService("set  params " + message.ToReadableString());
            }
            else
            {
                DebugService("not a get not a set");
            }
        }
        else
        {
            DebugService("transform not found " + id);
        }
    }
    // void ReportTRS(WSServiceBehaviour beh, Transform thisobj, System.UInt64 oid, bool useLocal = true, bool useRotation = false, bool useScale = false)
    // {
    //     OSCMessage message = new OSCMessage(Const.objectIDKeywordAddress + (useScale ? Const.objectPosRotScale : (useRotation ? Const.objectPosRot : useLocal ? Const.objectPosLocal : Const.objectPosGlobal)));
    //     Vector3 pos = useLocal ? thisobj.localPosition : thisobj.position;

    //     message.Append(pos.x);
    //     message.Append(pos.y);
    //     message.Append(pos.z);
    //     if (useRotation)
    //     {
    //         Quaternion rot = useLocal ? thisobj.localRotation : thisobj.rotation;
    //         message.Append(rot.x);
    //         message.Append(rot.y);
    //         message.Append(rot.z);
    //         message.Append(rot.w);

    //     }
    //     if (useScale)
    //     {
    //         Vector3 localScale = thisobj.localScale;
    //         message.Append(localScale.x);
    //         message.Append(localScale.y);
    //         message.Append(localScale.z);
    //     }
    //     beh.Send(message);
    // }
    // void Update()
    // {
    //     // UpdateQueues();
    //     // if (Time.time > nextUpdateTime)
    //     // {
    //     //     nextUpdateTime = Time.time + minimalUpdateTime;
    //     //     UpdateProxies();
    //     // }

    // }
}
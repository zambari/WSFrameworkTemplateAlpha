using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityOSC;
using WSFrameworkConst;

public static class TransformOSCExtensions
{
    public static OSCMessage QueryState(this ulong id, bool local, bool position, bool rotation, bool scale)
    {
        string addres = Const.get + (local ? Const.local : Const.global) + (scale ? Const.posRotScale : (rotation ? (position ? Const.positionRotation : Const.rotation) : Const.position));
        OSCMessage response = new OSCMessage(addres);
        response.Append(id);
        return response;
    }
    public static OSCMessage ReportState(ulong id, bool local, bool position, bool rotation, bool scale)
    {
        Transform t = id.FindTransform();
        return ReportState(t, local, position, rotation, scale, id);

    }
    public static OSCMessage ReportState(this Transform transform, bool local, bool position, bool rotation, bool scale, ulong id = 0)
    {
        string addres = Const.set + (local ? Const.local : Const.global) + (scale ? Const.posRotScale : (rotation ? (position ? Const.positionRotation : Const.rotation) : Const.position));
        OSCMessage response = new OSCMessage(addres);
        // Debug.Log("building response  addres " + addres + " local " + local + "  position" + position + "  rotation " + rotation + " scale  " + scale);
        if (rotation == false && scale == false) position = true;
        if (id == 0)
        {
            id = transform.GetID();
        }
        response.Append(id);
        if (position)
        {
            if (local)
            {
                response.Append(transform.localPosition.x);
                response.Append(transform.localPosition.y);
                response.Append(transform.localPosition.z);
            }
            else
            {
                response.Append(transform.position.x);
                response.Append(transform.position.y);
                response.Append(transform.position.z);
            }
        }
        if (rotation)
        {
            if (local)
            {
                response.Append(transform.localRotation.x);
                response.Append(transform.localRotation.y);
                response.Append(transform.localRotation.z);
                response.Append(transform.localRotation.w);
            }
            else
            {
                response.Append(transform.rotation.x);
                response.Append(transform.rotation.y);
                response.Append(transform.rotation.z);
                response.Append(transform.rotation.w);
            }
        }
        if (scale)
        {
            response.Append(transform.localScale.x);
            response.Append(transform.localScale.y);
            response.Append(transform.localScale.z);

        }
        return response;
    }

    public static void ApplyState(this Transform transform, OSCMessage message)
    {
        string address = message.Address;

        if (address.StartsWith("/set"))
            address = address.OSCFollowingSemgents();
        bool local = false;
        if (address.StartsWith(Const.global))
        {
            local = false;
        }
        else
        {
            local = true;
        }
        address = address.OSCFollowingSemgents();
        if (address.StartsWith(Const.position))
        {
            Vector3 position = new Vector3(message.GetFloat(1), message.GetFloat(2), message.GetFloat(3));
            if (local)
                transform.localPosition = position;
            else
                transform.position = position;
        }
        else
        if (address.StartsWith(Const.rotation))
        {
            Quaternion rotation = new Quaternion(message.GetFloat(1), message.GetFloat(2), message.GetFloat(3), message.GetFloat(4));
            if (local)
                transform.localRotation = rotation;
            else
                transform.rotation = rotation;
        }
        else
        if (address.StartsWith(Const.positionRotation))
        {
            Vector3 position = new Vector3(message.GetFloat(1), message.GetFloat(2), message.GetFloat(3));
            Quaternion rotation = new Quaternion(message.GetFloat(4), message.GetFloat(5), message.GetFloat(6), message.GetFloat(7));
            if (local)
            {
                transform.localPosition = position;
                transform.localRotation = rotation;
            }
            else
            {
                transform.position = position;
                transform.rotation = rotation;
            }
        }
        else
        // if (address.StartsWith(Const.positionRotation))
        // {
        //     Vector3 position = new Vector3(message.GetFloat(1), message.GetFloat(2), message.GetFloat(3));
        //     Quaternion rotation = new Quaternion(message.GetFloat(4), message.GetFloat(5), message.GetFloat(6), message.GetFloat(7));
        //     if (local)
        //     {
        //         transform.localPosition = position;
        //         transform.localRotation = rotation;
        //     }
        //     else
        //     {
        //         transform.position = position;
        //         transform.rotation = rotation;
        //     }
        // }
        if (address.StartsWith(Const.posRotScale))
        {
            Vector3 position = new Vector3(message.GetFloat(1), message.GetFloat(2), message.GetFloat(3));
            Quaternion rotation = new Quaternion(message.GetFloat(4), message.GetFloat(5), message.GetFloat(6), message.GetFloat(7));
            Vector3 scale = new Vector3(message.GetFloat(8), message.GetFloat(9), message.GetFloat(10));
            transform.localPosition = position;
            transform.localRotation = rotation;
            transform.localScale = scale;
            transform.position = position;
            transform.rotation = rotation;
        }
        else
        if (address.StartsWith(Const.scale))
        {
            Vector3 scale = new Vector3(message.GetFloat(1), message.GetFloat(2), message.GetFloat(3));
            transform.localScale = scale;
        }
    }

}

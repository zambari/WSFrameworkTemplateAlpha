using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityOSC;
using WSFrameworkConst;
using Z;
using Z.Reflection;

public class WSComponentService : WSOSCService
{
    public bool selectLastObjectInEditor = true;

    protected override void OnOSCMessage(OSCMessage message, WSServiceBehaviour beh)
    {
        DebugService(message.BinaryData.ByteArrayToString(0, 0, '-'));
        if (!message.AssertTypeTag(0, 'u'))
        {
            DebugService("		first param is not ulong, aborintg ");
            return;
        }
        string fingerprint = message.GetULong(0).ToFingerprintString();
        string addres = message.Address;
        string restOfCommandd = addres.OSCFollowingSemgents();
        System.UInt64 id = message.GetULong(0);
        Transform targettedTransform = ObjectID.FindTransform(id);
        if (targettedTransform == null)
        {
            Debug.Log("not found " + id + " " + id.ToFingerprintString());
            ReportNotFound(beh, id);
            return;
        }
#if UNITY_EDITOR
        if (selectLastObjectInEditor)
        {
            UnityEditor.Selection.activeGameObject = targettedTransform.gameObject;
        }
#endif
        if (addres.StartsWith(Const.objectComponentsDetailsAddress))
        {
            var componentid = message.GetInt(1);

            Component component = BasicComponentInfo.GetFromComponetId(componentid);

            // Component component = targettedTransform.GetComponent(componentname);
            if (component == null)
            {
                DebugService("Component not found " + componentid);
            }
            else
                ReportComponentDetails(beh, targettedTransform, component, id, componentid);
        }
        else
        if (addres.StartsWith(Const.componentActive))
        {

            int comopnentId = message.GetInt(1);
            int val = message.GetInt(2);
            var c = BasicComponentInfo.GetFromComponetId(comopnentId);
            if (c == null)
            {
                Debug.Log("compoonentwith id " + comopnentId + " not found");
            }
            MonoBehaviour mono = c as MonoBehaviour;
            if (mono == null)
            {
                Debug.Log(" this component is not a monobeh");
            }
            else
            {
                mono.enabled = val == 1;
            }
        }
        else
        if (addres.StartsWith(Const.call))
        {

            int comopnentId = message.GetInt(1);
            int val = message.GetInt(2);

        }
        else
        if (addres == Const.objectComponentsAddress)
        {
            ReportComponents(beh, targettedTransform, id);
            return;
        }
        else 
        if (addres==Const.release)
        {
            
        }
    }

    void ReportComponentDetails(WSServiceBehaviour beh, Transform thisobj, Component c, System.UInt64 oid, int componentID)
    {
        DebugService("reporting component detail");
        if (c == null)
        {
            Debug.Log("invalid compnnet refernee");
        }
        OSCMessage message = new OSCMessage( Const.objectComponentsDetailsAddress);
        ComponentDescriptor descriptor = ComponentDescriptor.GetDescriptor(c.GetType());
        // brak slownika?
        var desc = descriptor.GetTransferableDescription(oid, componentID, c);
        desc.ReadValues(c);
        //desc.ReadValues(c);
        //var desc = descriptor.GetVisibleAsJson(oid, c);
        message.Append(oid);
        message.Append(componentID);
        message.Append(GetVisibleAsJson(desc));
        // DebugService(" sending json  " + desc);
        beh.Send(message);
    }

    string GetVisibleAsJson(ComponentDescriptorWithHandles descriptor) //used by service
    {
#if UNITY_EDITOR
        return JsonUtility.ToJson(descriptor, true); //faster in builds
#else
        return JsonUtility.ToJson(descriptor, false);
#endif
    }
    void ReportComponents(WSServiceBehaviour beh, Transform thisobj, System.UInt64 oid)
    {
        DebugService("reporting components");
        var allcomponents = thisobj.GetComponents<Component>();
        OSCMessage message = new OSCMessage( Const.objectComponentsAddress);
        GameObjectInfo info = new GameObjectInfo(thisobj); //=new GameObjectInfo();
        message.Append(JsonUtility.ToJson(info));
        // DebugService("sent " + JsonUtility.ToJson(info));
        beh.Send(message);
    }
    void ReportNotFound(WSServiceBehaviour beh, System.UInt64 oid)
    {
        OSCMessage message = new OSCMessage(Const.oscMessageAddess);
        message.Append(oid);
        beh.Send(message);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;
using WebSocketSharp.Server;

public class WSFakeServiceBehavior : WebSocketBehavior
{
    public CommStats stats;
    public System.Action<MessageEventArgs> onMessage;
    public System.Action<CloseEventArgs> onClose;
    public System.Action<ErrorEventArgs> onError;
    public System.Action onOpen;
    public string serviceName;
    protected override void OnMessage(MessageEventArgs e)
    {
        if (onMessage != null) onMessage(e);
        else
            Debug.Log("no listener");
        if (stats != null) stats.AddBytesRecieved(e.RawData.Length);
    }

    protected override void OnClose(CloseEventArgs e)
    {
        if (onClose != null) onClose(e);
    }
    protected override void OnError(ErrorEventArgs e)
    {
        if (onError != null) onError(e);
    }
    protected override void OnOpen()
    {
        if (onOpen != null) onOpen();
    }
}
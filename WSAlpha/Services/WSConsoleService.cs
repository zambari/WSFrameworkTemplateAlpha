using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WSConsoleService : WSOSCService
{
    public static WSConsoleService instance;
    void Awake()
    {
        if (instance == null || instance == this)
        {
            instance = this;
        }
    }
    public static void RemoteLog(string s)
    {
        OSCMessage message = new OSCMessage("/log/remote");
        message.Append(s);
        message.Append("no stack");
        for (int i = 0; i < instance.clientHanlders.Count; i++)
        {
            instance.clientHanlders[i].Send(message);
        }
    }
    protected override void OnOSCMessage(OSCMessage message, WSServiceBehaviour beh)
    {

    }
    void HandleLog(string logString, string stackTrace, LogType type)
    {
        string address = "/log";
        if (type == LogType.Log) address += "/normal";
        if (type == LogType.Error) address += "/error";
        if (type == LogType.Exception) address += "/exception";
        if (type == LogType.Warning) address += "/warning";
        OSCMessage message = new OSCMessage(address);
        message.Append(logString);
        message.Append(stackTrace);
        for (int i = 0; i < clientHanlders.Count; i++)
        {
            clientHanlders[i].Send(message);
        }
    }

    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }
    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;

    }


}

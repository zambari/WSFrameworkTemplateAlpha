using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WSConsoleClient : WSOSCClient
{
    public bool printNormal = false;
    public bool printErrors = true;
    public bool printExceptions = true;
    protected override void OnOSCMessage(OSCMessage message)
    {
        string addess = message.Address;
        string payload = message.GetString(0);
        string stackTrace = message.GetString(1);
        string logtype = addess.OSCFollowingSemgents();
        
        if (logtype == "/normal")
        {
            if (!printNormal) return;
            logtype = null;
        }
        else
            if (logtype == "/error")
        {
            if (!printErrors) return;
        }
        else
            if (logtype == "/exception")
        {
            if (!printExceptions) return;
        }
        else
        {
            if (logtype == "/error")
            {
                if (!printErrors) return;
            }
        }
          if (logtype == "/remote")
          {
              Debug.Log((" Remote Log: " + logtype + " " + payload + " " + stackTrace).MakeBlue().Large());
          }
           else 
        Debug.Log((" Remote Log: " + logtype + " " + payload + " " + stackTrace).MakeBlue());

    }



}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using zUI;

public static class WSTargetExtensions
{
    public static RectTransform AddTopMenu(this WSTargetAddress target)
    {
        //if (target==)
        //      Canvas c = GameObject.FindObjectOfType(typeof(Canvas)) as Canvas;
        var top = UIBuilder.BuildMenuBar(target.transform as RectTransform);

        var pingstatus = UIBuilder.BuildEmptyRect(top);
        pingstatus.sizeDelta += Vector2.up * 5;
        var pingstatusText = pingstatus.AddChild().FillParent().gameObject.AddComponent<Text>();
        pingstatusText.name = "TextStatus";
        pingstatusText.text = "pingstatus";
        UIBuilder.SetDefaultFont(pingstatusText);
        var connFlag = UIBuilder.BuildEmptyRect(top);
        connFlag.GetComponent<Image>().enabled = false;
        connFlag.SetColor(Color.green);
        var connFlagred = UIBuilder.BuildEmptyRect(top);
        connFlagred.GetComponent<Image>().enabled = true;
        connFlagred.SetColor(Color.red);
        var conle = connFlag.gameObject.AddOrGetComponent<LayoutElement>();
        conle.preferredWidth = 40;
        conle.preferredHeight = 40;
        conle = connFlagred.gameObject.AddOrGetComponent<LayoutElement>();
        conle.preferredWidth = 40;
        conle.preferredHeight = 40;
        connFlagred.SetFlexibleWidth(0);
        connFlag.SetColor(Color.green);
        connFlag.SetPreferreedWidth(40);
        // connFlag.SetFlexibleWidth(0);

        //  var constatus = UIBuilder.BuildEmptyRect(top);
        var les = top.GetComponentsInChildren<LayoutElement>();
        foreach (var l in les)
            l.flexibleWidth = 0;
        var input = UIBuilder.BuildInputField(top, target.ipAddress, "Ipaddress");
        var input2 = UIBuilder.BuildInputField(top, target.portString, "Port");
        var connectionButton = UIBuilder.BuildButton(top, "Connect");
        var disconnect = UIBuilder.BuildButton(top, "Disconnect");
        input.text = target.ipAddress;
        input2.text = target.portString;
        input.GetComponent<RectTransform>().SetFlexibleWidth(0);
        input2.GetComponent<RectTransform>().SetFlexibleWidth(0);
        connectionButton.GetComponent<RectTransform>().SetFlexibleWidth(0);
        disconnect.GetComponent<RectTransform>().SetFlexibleWidth(0);
        connectionButton.onClick.AddListener(target.TryConnectingPingAndConnectRestIfSucceds);
        disconnect.onClick.AddListener(target.BroadcastDiscConnectionRequest);
        input.onValueChanged.AddListener((x) => target.ipAddress = x);
        input2.onValueChanged.AddListener((x) => target.portString = x);
        pingstatus.SetPreferreedHeight(20);
        var ping = target.gameObject.AddOrGetComponent<WSPingClient>();
        ping.OnConnection.AddListener((x) => connFlag.GetComponent<Image>().enabled = x);
        ping.OnConnection.AddListener((x) => disconnect.gameObject.SetActive(x));
        ping.OnConnection.AddListener((x) => connFlag.GetComponent<Image>().enabled = x);
        ping.OnDisconnection.AddListener((x) => connectionButton.gameObject.SetActive(x));
        ping.OnPingResult.AddListener((x) => pingstatusText.text = x);
        ping.OnDisconnection.AddListener((x) => input.gameObject.SetActive(x));
        ping.OnDisconnection.AddListener((x) => connFlagred.GetComponent<Image>().enabled = x);
        ping.OnDisconnection.AddListener((x) => input2.gameObject.SetActive(x));
        var layout = top.AddHorizontalLayout();
        layout.childAlignment = TextAnchor.MiddleCenter;
      //  retrun top;
      return top;
    }

}
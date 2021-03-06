﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityOSC;
using WebSocketSharp;
using Z;
//zbr 2020

public class WSCameraClient : WSOSCClient
{
    Texture2D texture;
    public RawImage targetImage;

    [Range(0, 3)]
    public float extraRequestinterval = .13f;

    public Vector2Int resolution = new Vector2Int(40, 40);
    public int jpgQuality = 70;

    Coroutine requester;
    public bool automaticallyRequest = true;
    protected override void Reset()
    {
        base.Reset();
        targetImage = GetComponent<RawImage>();
    }

    protected override void OnConnected()
    {
        base.OnConnected();
        SendConfig();
    }
    IEnumerator RequesterRoutine()
    {
        yield return null;
        yield return null;

        while (true)
        {
            if (IsConnected && automaticallyRequest)
                SendRequest();
            yield return null;
            yield return new WaitForSeconds(extraRequestinterval);
        }
    }

    [ExposeMethodInEditor]
    void SendConfig()
    {
        Send(WSCameraService.oscSetResolution.WrapAsOscPayload(resolution.x, resolution.y, jpgQuality));
        // DebugClient("Sent setres /setResolution " + WSCameraService.oscSetResolution);
    }

    [ExposeMethodInEditor]
    void SendRequest()
    {
        Send(WSCameraService.oscRequestFrame.WrapAsOscPayload());
        // DebugClient("Seent requeqst " + WSCameraService.oscRequestFrame);
    }

    private void OnEnable()
    {
        DebugClient("camera cleine bta");

        requester = StartCoroutine(RequesterRoutine());
    }
    private void OnDisable()
    {
        StopCoroutine(requester);
    }

    protected override void OnOSCMessage(OSCMessage message)
    {
        // DebugClient("recienig mes addr " + message.Address + " type " + message.typeTag);
        if (message.Address == WSCameraService.oscFrameAddress)
        {
            byte[] data = message.GetBytes();
        
            if (texture == null)
                texture = new Texture2D(1, 1);
            if (texture.LoadImage(data))
            {
                targetImage.texture = texture;
            }
        }
        else
        {
            DebugClient("unknown address " + message.Address);
        }
    }
}
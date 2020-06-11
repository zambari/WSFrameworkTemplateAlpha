using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityOSC;
using WebSocketSharp;
using Z;
//zbr 2020

public class WSCameraService : WSOSCService
{
	new Camera camera { get { if (_camera == null) _camera = GetComponent<Camera>(); return _camera; } }
	Camera _camera;
	// Use this for initialization

	// protected override void OnMessageDequeue(WSServiceBehaviour beh, MessageEventArgs s)
	// {
	// 	DebugService(" responding with frame :" + s.Data);
	// 	SendFrame(beh);
	// }
	public static string oscSetResolution { get { return "/setResolution"; } }
	public static string oscRequestFrame { get { return "/requestFrame"; } }
	public static string oscFrameAddress { get { return "/frame"; } }

	[Range(1, 100)]
	public int quality = 90;
	public Vector2Int textureResolutoin = new Vector2Int(640, 480);
	// false, meaning no need for mipmaps
	Texture2D texture;
	RenderTexture rt;
	public bool captureMainCamera;

	[ReadOnly]
	public int ticksSpentEncodingJpgs;

	[ReadOnly]
	public float averageTimeSpending;
	// public int ticksSpentEncodingJpgs;
	// [ExposeMethodInEditor]
	// void broadcastsomethit()
	// {
	// 	BroacdcastString("shit");
	// }
	// [ExposeMethodInEditor]
	// void BroadcastsomethitBy()
	// {
	// 	byte[] allbytes = new byte[254];
	// 	for (int i = 0; i < allbytes.Length; i++) allbytes[i] = (byte) i;
	// 	BroacdcastBytes(allbytes.WrapAsOscPayload(oscFrameAddress));
	// }
	public void SetResolution(int x, int y, int q)
	{
		textureResolutoin = new Vector2Int(x, y);
		quality = q;
	}
	protected override void OnOSCMessage(OSCMessage message, WSServiceBehaviour beh)
	{
		if (message.Address == oscRequestFrame)
		{
			SendFrame(beh);
		}
		else
		if (message.Address == oscSetResolution)
		{
			SetResolution(message.GetInt(0), message.GetInt(1), message.GetInt(2));
		}
	}
	public void SendFrame(WSServiceBehaviour beh)
	{
		System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
		stopwatch.Start();
		if (captureMainCamera)
			rt = camera.targetTexture;

		if (!rt.CheckDimensions(textureResolutoin))
		{
			rt = new RenderTexture(textureResolutoin.x, textureResolutoin.y, 8);
			rt.name = textureResolutoin.ToString();
		}
		//	var oldRT = RenderTexture.active;
		camera.Render();
		Graphics.Blit(camera.targetTexture, rt);
		RenderTexture.active = rt;
		if (!texture.CheckDimensions(rt))
		{
			texture = new Texture2D(rt.width, rt.height);
			texture.name = textureResolutoin.ToString();
		}
		texture.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
		texture.Apply();

		byte[] bytes = texture.EncodeToJPG(quality);

		OSCMessage message = new OSCMessage(oscFrameAddress);
		message.Append(bytes);
		beh.SendAsync(message.BinaryData, null);

		stopwatch.Stop();
		ticksSpentEncodingJpgs = (int) stopwatch.ElapsedTicks;
		stats.AddBytesSent(message.BinaryData.Length);
		float r = 0.8f;
		averageTimeSpending = r * averageTimeSpending + (1 - r) * (stopwatch.ElapsedMilliseconds);

	}

}
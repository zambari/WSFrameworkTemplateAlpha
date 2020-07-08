using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityOSC;
using WebSocketSharp;
using WSFrameworkConst;
using Z;
//zbr 2020

public class WSCameraService : WSOSCService
{
    new Camera camera { get { if (_camera == null) _camera = GetComponent<Camera>(); return _camera; } }
    public Camera _camera;
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
    // protected override void OnOpen(WSServiceBehaviour serviceBehaviour)
    // {

    // }

    public void SetResolution(int x, int y, int q)
    {
        CheckCamera();
        textureResolutoin = new Vector2Int(x, y);

        if (camera.targetTexture == null ||
            camera.targetTexture.width < x || camera.targetTexture.height < y)
        {
            var rt = new RenderTexture(x, y, 8);
            Debug.Log("created a bigger texture");
            camera.targetTexture = rt;
        }
        quality = q;
    }
    protected override void OnOSCMessage(OSCMessage message, WSServiceBehaviour beh)
    {
        string address = message.Address;
        if (address == oscRequestFrame)
        {
            SendFrame(beh);
        }
        else
        if (address == oscSetResolution)
        {
            DebugService("message is  seteres " + message.ToReadableString());
            SetResolution(message.GetInt(0), message.GetInt(1), message.GetInt(2));
        }
        else
        if (address.StartsWith(Const.set + Const.positionRotation))
        {
            if (targetSmooth != null)
            {
                targetSmooth.ApplyState(message);
                // Debug.Log("applying");
            }
            else
            {
                Debug.Log("no target");
            }
        }
        else
        {
            DebugService("unkonown comand");
        }
    }
    protected override void Start()
    {
        base.Start();

    }

    SmoothFollower smoothFollower;
    Transform targetSmooth;
    void CheckCamera()
    {
        if (camera == null)
        {
            GameObject pivot = new GameObject("WSCamPivot");
            GameObject cam = new GameObject("WSCamera");
            _camera = cam.gameObject.AddComponent<Camera>();
            cam.transform.SetParent(pivot.transform);
            cam.transform.localScale=Vector3.one;
            cam.transform.localPosition=Vector3.forward*-12;
            // GameObject smoothf = new GameObject("smootharget");
            // targetSmooth = smoothf.transform;
            // smoothFollower = cam.AddComponent<SmoothFollower>();
            // smoothFollower.positionSource = targetSmooth;
            // cam.transform.localPosition = -Vector3.forward * 3;
            //smoothFollower.

        }
        if (camera.targetTexture == null)
        {
            var rt = new RenderTexture(640, 480, 8);
            camera.targetTexture = rt;
        }
    }
    protected override void OnOpen(WSServiceBehaviour serviceBehaviour)
    {

        CheckCamera();

    }
    public bool useNonBlockingEncoder;
    JPGEncoder encoder;
    System.Diagnostics.Stopwatch stopwatch;
    bool isEncoding;
    WSServiceBehaviour pendingBehavior;
    public void SendFrame(WSServiceBehaviour beh)
    {
        if (isEncoding)
        {
            Debug.Log("is already encoding");
            return;
        }
        pendingBehavior = beh;
        stopwatch = new System.Diagnostics.Stopwatch();
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

        if (onRawTextureReady != null)
        {
            var bytes = texture.GetRawTextureData();
            Debug.Log(texture.format);
            onRawTextureReady.Invoke(bytes);
            return;
        }
        else
        {
            isEncoding = false;

            byte[] bytes = texture.EncodeToJPG(quality);
            SendJpg(bytes, pendingBehavior);
        }
    }
    public System.Action<byte[]> onRawTextureReady;
    private void DoEncodingBg()
    {
        byte[] bytes = texture.EncodeToJPG(quality);
        SendJpg(bytes, pendingBehavior);
    }
    public void onMessage(MessageEventArgs e)
    {
        if (clientHanlders.Count > 0)
        {
            OSCMessage message = new OSCMessage(WSCameraService.oscFrameAddress);
            message.Append(e.RawData);
            clientHanlders[0].SendAsync(e.RawData, null);
            Debug.Log("forwarding back");
        }
    }
    void SendJpg(byte[] bytes, WSServiceBehaviour beh)
    {
        stopwatch.Stop();
        OSCMessage message = new OSCMessage(oscFrameAddress);
        message.Append(bytes);
        beh.SendAsync(message.BinaryData, null);

        ticksSpentEncodingJpgs = (int) stopwatch.ElapsedTicks;
        float r = 0.8f;
        averageTimeSpending = r * averageTimeSpending + (1 - r) * (stopwatch.ElapsedMilliseconds);
        stats.AddBytesSent(message.BinaryData.Length);
    }
    void OnEncodeReady(byte[] bytes)
    {
        isEncoding = false;
        SendJpg(bytes, pendingBehavior);
    }

    // private void Update() 
    // 	{
    // 		if(Input.GetKeyDown("p"))
    // 			StartCoroutine("ScreenshotEncode");
    // 	}
    // 	private IEnumerator ScreenshotEncode()
    // 	{
    // 		yield return new WaitForEndOfFrame();

    // 		// create a texture to pass to encoding
    // 		texture = new Texture2D (Screen.width, Screen.height, TextureFormat.RGB24, false);

    // 		// put buffer into texture
    // 		texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
    // 		texture.Apply(false, false);

    // 		string fullPath = Application.dataPath + "/../testscreen-" + count + ".jpg";

    // 		//How to encode without save to disk
    // //		JPGEncoder encoder = new JPGEncoder(texture, 75);

    // 		//encoder is threaded; wait for it to finish
    // 		while(!encoder.isDone)
    // 			yield return null;

    // //		How to use the JPG encoder as a blocking method. This way it isn't needed to wait for it to finish
    // //		But the unity main thread will freeze while the encoding takes place (just like Texture.EncodeToPNG() does)
    // //		JPGEncoder encoder1 = new JPGEncoder(texture, 75, true);

    // 		Debug.Log("Screendump saved at : " + fullPath);
    // 		Debug.Log("Done encoding and bytes ready for use. e.g. send over network, write to disk");
    // 		Debug.Log("Size: " + encoder.GetBytes().Length + " bytes");

    // 		count++;

    // 	}

}
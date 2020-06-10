using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class oscCameraProvider : MonoBehaviour
{
	new Camera camera { get { if (_camera == null) _camera = GetComponent<Camera>(); return _camera; } }
	Camera _camera;
	public ISendOSCSelector senderSelector;
	public Vector2Int targetResolution = new Vector2Int(320, 240);
	private void OnValidate()
	{
		senderSelector.OnValidate(this);

	}

	[Range(0, 100)]
	public int quality = 60;
	RenderTexture rt;
	private void OnPostRender()
	{
		if (RenderTexture.active != null)
		{
			// Debug.Log("rt is " + RenderTexture.active.width + " " + RenderTexture.active.height);
			if (!rt.CheckDimensions(targetResolution)) rt = new RenderTexture(targetResolution.x, targetResolution.y, 8);
			Graphics.Blit(RenderTexture.active, rt);
			var oldRT = RenderTexture.active;
			RenderTexture.active = rt;
			var tex = new Texture2D(rt.width, rt.height);
			tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
			tex.Apply(); ///neede ?
			var bytes = tex.EncodeToJPG(quality);
			Debug.Log(" encoded to " + bytes.Length);
			RenderTexture.active = oldRT;
		}
		else Debug.Log("notexteure");
	}

	[ExposeMethodInEditor]
	void SendTexture()
	{
		if (senderSelector.valueSource != null) senderSelector.valueSource.Send("/test message");
	}

}
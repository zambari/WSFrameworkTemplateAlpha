using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityOSC;

public class WSPointCloudClient : WSOSCClient
{
	public Vector2Int rangeX;
	public Vector2Int rangeY;
	Vector2Int dimensions;
	public float scale = 10;
	protected override void OnOSCMessage(OSCMessage message)
	{
		// DebugClient("recienig mes addr " + message.Address + " type " + message.typeTag);
		if (message.Address == "/depth")
		{
			dimensions = new Vector2Int(message.GetInt(0), message.GetInt(1));
			byte[] data = message.GetBytes(2);

			CreateParticles(data);
			if (points == null || points.Length != dimensions.x * dimensions.y)
			{
				points = new ParticleSystem.Particle[dimensions.x * dimensions.y];
				ClearParticles();
			}
			//	Debug.Log("dimensionsa re " + dimensions + " len = " + data.Length + " should be " + dimensions.x * dimensions.y * 2);
		}
		else
		{
			DebugClient("unknown address " + message.Address);
		}
	}

	// Use this for initialization
	new protected ParticleSystem particleSystem { get { if (_particleSystem == null) _particleSystem = GetComponentInChildren<ParticleSystem>(); return _particleSystem; } }
	protected ParticleSystem _particleSystem;
	ParticleSystem.Particle[] points;
	protected override IEnumerator Start()
	{
		particleSystem.Stop();

		yield return base.Start();
		//StartCoroutine(
		// Invoke("CreateParticles", 1);
	}
	public float particleSize { get { return _particleSize; } set { _particleSize = value; ClearParticles(); } }

	[Range(0, 0.5f)]
	public float _particleSize = 0.03f;
	public float scaleX { get { return transform.localScale.x; } set { transform.localScale = new Vector3(value, transform.localScale.y, transform.localScale.z); } }
	public float scaleY { get { return transform.localScale.y; } set { transform.localScale = new Vector3(transform.localScale.x, value, transform.localScale.z); } }
	// public RsPointSampler pointSampler;
	public float depthScale { get { return _depthScale; } set { _depthScale = value; } }
	public float _depthScale = 10;
	[Range(0, .07f)]
	[SerializeField] float depthBias = 0;
	void ClearParticles()
	{
		if (points == null) return;
		for (int i = 0; i < points.Length; i++) points[i].startSize = 0;
	}
	float lastSaneValue;
	float preliminaryDivision = 1 / 30000f;

	[Range(0, 0.026f)]
	[SerializeField] float _zeroTreshold = 0.006f;
	public float zeroTreshold
	{
		get { return _zeroTreshold; }
		set { _zeroTreshold = value; }
	}
	public float GetValue(int x, int y, byte[] bytes)
	{
		//   return GetValue((y * textureDimensions.x) + x);
		if (rangeX == Vector2Int.zero) rangeX = new Vector2Int(0, x);
		if (rangeY == Vector2Int.zero) rangeY = new Vector2Int(0, y);
		int thisIndex = zExtensionsTextures.GetIndex(x, y, dimensions);
		int indexDoubled = thisIndex * 2;
		if (thisIndex < 0) Debug.Log("neg index");
		if (thisIndex < 0 || indexDoubled >= bytes.Length - 1)
			return lastSaneValue;
		float thisReadout = preliminaryDivision * ((int) (((int) bytes[indexDoubled + 1] << 8 | ((int) bytes[indexDoubled]))));
		if (thisReadout < zeroTreshold) // || Mathf.Abs(lastSaneValue - thisReadout) > deltathresh
			thisReadout = lastSaneValue;
		else
		{
			lastSaneValue = thisReadout;
		}
		return lastSaneValue;

		// if (index >= savedValues.Length)
		// {
		//     Debug.Log($"  (index  {index}>= savedValues.Length {savedValues.Length}");

		// }
		// var val = GetValueRaw(index);
		// if (substractBg && savedValues != null && savedValues.Length >= index)
		//     val -= savedValues[index];
	}
	public Vector3 GetValue3D(int x, int y, byte[] bytes)
	{
		// int index = zExtensionsTextures.GetIndex(x, y,dimensions);
		float val = GetValue(x, y, bytes);
		//     float val = GetValueRaw(index);
		//    if (substractBg)
		///       val -= savedValues[index];
		var vect = new Vector3((x - dimensions.x * .5f) / dimensions.x, (y - dimensions.y * .5f) * 1f / dimensions.y, val - .5f);
		return vect * scale;
		//    return GetValue(((textureDimensions.y - y + 1) * textureDimensions.x) - x);
	}
	bool substractBg = false;
	public Vector2 offset;
	[ExposeMethodInEditor]
	void CreateParticles(byte[] bytes)
	{

		var baseParticle = new ParticleSystem.Particle();
		baseParticle.remainingLifetime = 10;
		Color basec = Color.white * 0.6f;
		basec.a = 0.1f;
		baseParticle.startColor = basec; //: Color.white;
		baseParticle.startSize = particleSize;
		Color selColor = Color.green;
		if (points == null || points.Length != dimensions.x * dimensions.y)
			points = new ParticleSystem.Particle[dimensions.x * dimensions.y];
		int partindex = 0;

		for (int j = rangeY.x; j <= rangeY.y; j++)
		{
			float y = j * 1f / dimensions.y - 0.5f;
			for (int i = rangeX.x; i <= rangeX.y; i++)
			{
				float x = i * 1f / dimensions.x - .5f;
				float z = GetValue(i, j, bytes);
				Vector3 position = substractBg ? new Vector3(x - offset.x, y - offset.y, depthScale * (z - depthBias)) : new Vector3(x - offset.x, y - offset.y, depthScale * z);
				position *= scale;
				points[partindex] = baseParticle;
				points[partindex].position = position;
				// if (j > pointSampler.rangeY.x && j < pointSampler.rangeY.y)
				// 	if (i > pointSampler.rangeX.x && i < pointSampler.rangeX.y)
				// 	{
				// 		points[partindex].startColor = selColor;
				// 		points[partindex].startSize *= 2;
				// 	}
				partindex++;
			}
		}

		particleSystem.SetParticles(points, points.Length);
		// Debug.Log("setparts");
	}

}
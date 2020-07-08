using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridFlasher : MonoBehaviour
{

	public Material material;
	public bool instantiate = true;
	public AnimationCurve flashCurve = zExt.LinearCurveDown();
	[Range(0.05f, 20)]
	public float flashInterval = 1;
	// public bool useFrameDuration;
	public Color startColor = Color.white;
	 float phase;
	public string colorPropertyName = "_Color";
	[Range(0.1f, 4)]
	public float fadeSpeed = .7f;
	void Reset()
	{
		TryGettingMaterial();
		flashCurve = zExt.LinearCurveDown();
	}

	void TryGettingMaterial()
	{
		var mr = GetComponentInChildren<MeshRenderer>();
		if (mr == null && Application.isPlaying)
		{
			this.enabled = false;
			return;
		}
		if (instantiate)
		{
			material = mr.material;
			mr.material = material;
		}
		else
		{
			material = mr.sharedMaterial;
		}

	}
	void Start()
	{
		if (material == null)
			TryGettingMaterial();
	}
	private void OnValidate()
	{
		if (string.IsNullOrEmpty(colorPropertyName)) colorPropertyName = "_Color";
	}
	float nextFlashTime;
	[ExposeMethodInEditor]
	void ResetTimer()
	{
		nextFlashTime = Time.time;

	}
	void Update()
	{
		if (material == null)
		{
			this.enabled = false;
			return;
		}
		if (Time.time >= nextFlashTime)
		{
			nextFlashTime = Time.time + flashInterval;
			phase = 0;
		}
		if (phase < 1)
		{
			Color color = startColor;
			color.a = flashCurve.Evaluate(phase);
			material.SetColor(colorPropertyName, color);
			phase += Time.deltaTime * fadeSpeed;

		}
	}
}
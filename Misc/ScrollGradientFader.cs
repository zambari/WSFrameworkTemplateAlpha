using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Z;
public class ScrollGradientFader : ScrolPooledControllerBase
{
	protected override void Reset()
	{
		base.Reset();
		SetPreset1();
	}
	public override int GetCount()
	{
		return 0;
	}

	[Header("Gradient")]
	public bool useAlternativeAlpha=true;
	[Range(0, 1f)]
	public float alternativeAlpha=1;
	public Color baseColor = new Color(12 / 245f, 69 / 256f, 81 / 256f);
	[Header("Gradient")]
	public bool useGradient = true;
	public Gradient colorGradient = zExt.HeatGradient();

	[Header("HueShift")]
	public bool useHueShift;
	[Range(-1f, 1f)]
	public float hueShiftAmount;

	[Header("ConstantHueShift")]

	public bool useconstantHueShift;
	[Range(-1f, 1f)]
	public float constantHueShiftAmount;

	[Header("HueShift")]
	public bool useConstantSatShift;
	[Range(-1f, 1f)]
	public float constantSatAmount;
	[Header("BrigttnessOffset")]
	public bool useBrigtnessOffset;
	public float brigtnessOffset;
	[Header("AlphtaOffset")]
	public bool useAlphtaOffset;
	public float alphaOffset;
	[Header("Values")]
	[Range(5, 100)]
	public int period = 10;
	public bool pingpong = true;
	float GetPoint(int index)
	{
		if (pingpong)
		{
			index = index % (period * 2);
			if (index > period) index = 2 * period - index;
			return (index) * 1f / period;
		}
		else
		{
			return (index % period) * 1f / period;
		}
	}
	private void OnValidate()
	{
		if (Application.isPlaying && scrollPooled != null)
			scrollPooled.UpdateFill();
			useAlternativeAlpha=true;
			alternativeAlpha=0.8f;
	}

	public override void OnFillItem(int index, GameObject go)
	{
		var item = go.GetComponent<ListItem>();
		if (item == null || item.image == null) return;
		float point = GetPoint(index);
		Color color = useGradient?colorGradient.Evaluate(point) : baseColor;
		if (useHueShift)
			color = color.ShiftHue(hueShiftAmount * point);

		if (useconstantHueShift)
			color = color.ShiftHue(constantHueShiftAmount);
		if (useBrigtnessOffset)
			color = new Color(color.r + brigtnessOffset, color.g + brigtnessOffset, color.b + brigtnessOffset, color.a);
		if (useAlphtaOffset)
			color = new Color(color.r, color.g, color.b, color.a + alphaOffset);
		if (useAlternativeAlpha)
		{
			if (index % 2 == 0)
				color.a *= alternativeAlpha;
		}
		if (useConstantSatShift)
		{
			color = color.ShiftSat(constantSatAmount);
		}
		item.image.color = color;
	}

	[ExposeMethodInEditor]
	void Dump()
	{
		colorGradient.DumpKeys("colorGradient");
	}

	[ExposeMethodInEditor]
	void SetPreset1()
	{
		var colorKey = new GradientColorKey[] { new GradientColorKey(new Color(0.04705883f, 0.2705882f, 0.3176471f, 1f), 0f), new GradientColorKey(new Color(0f, 0.2830189f, 0.1233004f, 1f), 0.2088197f), new GradientColorKey(new Color(0.3584906f, 0.2755681f, 0.05580278f, 1f), 0.4705882f), new GradientColorKey(new Color(0.3962264f, 0.1738163f, 0.2324106f, 1f), 0.8117647f), new GradientColorKey(new Color(0.04705883f, 0.2705882f, 0.3176471f, 1f), 1f) };
		var alphaKey = new GradientAlphaKey[] { new GradientAlphaKey(1, 0), new GradientAlphaKey(1, 1) };
		colorGradient.SetKeys(colorKey, alphaKey);
	}

	[ExposeMethodInEditor]
	void SetPreset2()
	{
		var colorKey = new GradientColorKey[] { new GradientColorKey(new Color(0.3204536f, 0.6886792f, 0.2306426f, 1f), 0f), new GradientColorKey(new Color(0.4875148f, 0.5660378f, 0.1682093f, 1f), 0.197055f), new GradientColorKey(new Color(0.8396226f, 0.4710617f, 0.1703008f, 1f), 0.4323491f), new GradientColorKey(new Color(0.3035615f, 0.5339625f, 0.4679084f, 1f), 0.7147021f), new GradientColorKey(new Color(0.2714044f, 0.5377358f, 0.4857613f, 1f), 0.8323491f), new GradientColorKey(new Color(0.3215686f, 0.6901961f, 0.2313726f, 1f), 1f) };
		var alphaKey = new GradientAlphaKey[] { new GradientAlphaKey(1, 0), new GradientAlphaKey(1, 1) };
		colorGradient.SetKeys(colorKey, alphaKey);
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Z.Reflection;

public class ValueRemote
{
	Slider slider;
	ValueProxy link;
	public int memberId { get { return link.memberId;}}
	public void UpdateValue(float f)
	{
		if (slider==null)
		{
			Debug.Log("no slider");
		}else
		{
			slider.value=f;
		}
	}
	public void BindSlider(Slider slider)
	{
		this.slider = slider;
		if (link.valueRange != Vector2.zero)
		{
			slider.minValue = link.valueRange.x;
			slider.maxValue = link.valueRange.y;

		}
		slider.value = link.lastValue;
		slider.onValueChanged.AddListener(OnSliderMove);
		slider.SetLabel(link.baseName);
	}
	void OnSliderMove(float f)
	{
		WSValueClient.RequestValueChange(memberId, f);
	}

	public ValueRemote(ValueProxy link)
	{
		this.link = link;
	//	this.valueId = link.objectIdTarget;

	}
	public static void RegisterRemote(ulong v, ValueRemote remote)
	{
		if (valueRemoteDict == null) valueRemoteDict = new Dictionary<ulong, ValueRemote>();
		valueRemoteDict.Add(v, remote);
	}

	public static ValueRemote GetRemote(ulong v)
	{
		ValueRemote remote = null;
		if (valueRemoteDict.TryGetValue(v, out remote))
		{

		}
		return remote;
	}
	public static Dictionary<ulong, ValueRemote> valueRemoteDict;
}
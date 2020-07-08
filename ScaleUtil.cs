using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScaleUtil : MonoBehaviour
{

	Vector3 startScale;
	[Range(0.1f, 2)]
	[SerializeField]
	float _scale = 1;
	public float scale
	{
		get { return _scale; } set
		{
			//	if (value==0) return;
			if (startScale == Vector3.zero) startScale = transform.localScale;
			_scale = value;
			if (Application.isPlaying)
			{
				float s = _scale * _scale * _scale;
				transform.localScale = startScale * s;
			}
		}
	}

	void Start()
	{
		startScale = transform.localScale;

	}
	private void OnValidate()
	{
		scale = 1;
		if (_scale != 0)
			scale = _scale;
	}
}
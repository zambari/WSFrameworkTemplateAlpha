using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSLimiter : MonoBehaviour
{

	[Range(1, 120)]
	[SerializeField] int _fpsLimit = 40;
	public bool applyOnAwakeInEditor = true;
	public bool applyOnAwakeInPlayMode = false;
	public int fpsLimit
	{
		get { return _fpsLimit; } set
		{
			_fpsLimit = value;
			Application.targetFrameRate = fpsLimit;
		}
	}
	void Awake()
	{
#if UNITY_EDITOR
		if (applyOnAwakeInEditor) fpsLimit = fpsLimit;
#else
		if (applyOnAwakeInPlayMode) fpsLimit = fpsLimit;
#endif

	}
	private void OnValidate()
	{
		fpsLimit = fpsLimit;

	}
}
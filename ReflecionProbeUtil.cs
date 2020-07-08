using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReflecionProbeUtil : MonoBehaviour
{

	ReflectionProbe reflectionProbe { get { if (_reflectionProbe == null) _reflectionProbe = GetComponent<ReflectionProbe>(); return _reflectionProbe; } }

	[SerializeField] ReflectionProbe _reflectionProbe;

	[Range(0, 2)]
	[SerializeField] float _intentisty = 1;
	public float intentisty
	{
		get { return _intentisty; }
		set
		{
			_intentisty = value;
			reflectionProbe.intensity = _intentisty * _intentisty;
		}
	}

	void Start()
	{
		_intentisty = Mathf.Sqrt(reflectionProbe.intensity);

	}

	[Range(0, 1)]
	[SerializeField] float _reflcetionProbeNormalized;
	public float reflcetionProbeNormalized
	{
		get { return _reflcetionProbeNormalized; }
		set
		{
			_reflcetionProbeNormalized = value;
			SetRefProbe();
			//OnValidate();
		}
	}

	public Texture[] textures;
	void SetRefProbe()
	{
		int index = Mathf.FloorToInt(reflcetionProbeNormalized * textures.Length);
		if (index >= textures.Length) index = textures.Length - 1;
		if (index < 0) index = 0;
		// Debug.Log("index " + index);
		if (textures.Length > index && textures[index] != null)
		{
			reflectionProbe.bakedTexture = textures[index];
		}
		else
		{
			Debug.Log("isnull @" + index + " te " + textures.Length);
		}
		intentisty = intentisty;

	}

	[ExposeMethodInEditor]
	void OnValidate()
	{
		SetRefProbe();
	}

}
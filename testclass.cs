using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class testclass : MonoBehaviour
{
	public int x = 4;
	public float f1 = 53;
	public float f2
	{
		get { return _f2f; } set
		{
			_f2f = value;
			// Debug.Log("1set2b");
		}
	}
	void Start()
	{

	}
	public float _f2f;
	public string abnc = "strabnc";
	public string cds = "strcsd";
	public bool boolField;
	public bool boolGet { get { return Random.value > 0.5f; } set { Debug.Log("bool call " + value); } }
	public string StrGetSet { get { return _StrGetSet; } set { _StrGetSet = value; Debug.Log("String se called"); } }

	[SerializeField]
	string _StrGetSet = "getstrg";
	private void Reset()
	{
		x = Random.Range(0, 400);
		f1 = Random.Range(0, 5f);
		abnc = zExt.RandomString(5);
	}

	[ExposeMethodInEditor]
	void MyExposedMEthod()
	{
		Debug.Log("yaya");
	}
	private void OnValidate()
	{
		if (Application.isPlaying)
			Debug.Log("testclass validate");
	}
}
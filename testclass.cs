using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class testclass : MonoBehaviour {

	public float f1=53;
	public int x=4;
	public float f2 {get {return _f2; } set { _f2=value;Debug.Log("set"); }}
		public float _f2;
	public string abnc="Ytyt";
	public string cds="rere";
 private void Reset() {
	x=Random.Range(0,400);
	f1=Random.Range(0,5f);
	abnc=zExt.RandomString(5);
}
}

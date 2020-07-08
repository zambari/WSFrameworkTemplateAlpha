using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface ItemInterface
{
	void OnFillItem(int index, GameObject go);
	int GetCount();
	int GetHeight(int index);
// System.Func<int,int> GetH
}
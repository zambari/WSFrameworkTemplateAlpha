using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class ScrolPooledControllerBase : MonoBehaviour, ItemInterface
{

	[SerializeField]
	protected ScrollPooled scrollPooled;
	protected virtual void Reset()
	{
		scrollPooled = GetComponentInChildren<ScrollPooled>();
		if (scrollPooled == null)
			scrollPooled = GetComponentInParent<ScrollPooled>();

	}
	protected virtual void Awake()
	{
		if (scrollPooled == null)
			scrollPooled = GetComponentInChildren<ScrollPooled>();

		if (scrollPooled != null)
			scrollPooled.OnFill += OnFillItem;
		// ScrollPooled.InitList(Count, OnHeightItem);
	}
	public abstract void OnFillItem(int index, GameObject go);
	virtual public int GetHeight(int index)
	{
		return 0;
	}
	public abstract int GetCount();
}
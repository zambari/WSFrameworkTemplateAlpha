using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class DraggToRotate : MonoBehaviour, IDragHandler, IPointerClickHandler
{

	// Use this for initialization
	float lastClickTime;
	public float speedX = .2f;
	public float speedY = .2f;
	public Toggle modeToggle;
	public Transform rotateTranform;
	public Vector2 rotRange = new Vector2(70, 70);

	public void OnPointerClick(PointerEventData eventData)
	{
		if (Time.time - lastClickTime < 0.4f)
		{
			modeToggle.isOn = !modeToggle.isOn;
		}
		else
			lastClickTime = Time.time;
	}

	public void OnDrag(PointerEventData eventData)
	{
		Vector3 euler = rotateTranform.eulerAngles;
		euler.y += eventData.delta.x * speedX;
		euler.x += eventData.delta.y * speedY;
		//if (euler.x> rotRange.x) euler.x = rotRange.x;
	//	if (euler.x >360 -rotRange.x) euler.x = 360 -rotRange.x;
		//if (euler.y > rotRange.y) euler.y = rotRange.y;
		//if (euler.y >360 -rotRange.y) euler.y  =360 -rotRange.y;
		rotateTranform.eulerAngles = euler;
	}
}
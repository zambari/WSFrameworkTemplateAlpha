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
        if (modeToggle != null)
            if (Time.time - lastClickTime < 0.4f)
            {
                modeToggle.isOn = !modeToggle.isOn;
            }
            else
                lastClickTime = Time.time;
    }
    public float dragSpeed = 0.001f;
    public void OnDrag(PointerEventData eventData)
    {
        Vector3 euler = rotateTranform.eulerAngles;
        if (Input.GetMouseButton(2))
        {
            rotateTranform.position += new Vector3(dragSpeed * eventData.delta.x, dragSpeed * eventData.delta.y);
        }
        else
        {
            euler.y += eventData.delta.x * speedX;
            euler.x += eventData.delta.y * speedY;
            rotateTranform.eulerAngles = euler;

        }
        //if (euler.x> rotRange.x) euler.x = rotRange.x;
        //	if (euler.x >360 -rotRange.x) euler.x = 360 -rotRange.x;
        //if (euler.y > rotRange.y) euler.y = rotRange.y;
        //if (euler.y >360 -rotRange.y) euler.y  =360 -rotRange.y;

        Debug.Log("dragged");
    }
}
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine.UI;
// using UnityEngine;
// using Z.OSC;
// // zOSC_1 use example

// public class OSCSliderSendVector : MonoBehaviour
// {
//     //[Header("MODDED VERSION UWAGA")]
//     public string OSCName;
//     public Slider slider;
//     public Text valueDisplayText;
//     void OnValidate()
//     {
//         if (slider == null) slider = GetComponent<Slider>();
//         OSCName = OSCName.SanitizeOSCAddress();
//         if (valueDisplayText == null) valueDisplayText = GetComponentInChildren<Text>();
//     }

//     void Start()
//     {
//         slider.onValueChanged.AddListener(newSliderValue);
//     }

//     void newSliderValue(float f)
//     {
//         zOSC_1.BroadcastOSC(OSCName, new Vector3(f, f, f));
//         if (valueDisplayText != null) valueDisplayText.text = (Mathf.Round(f * 10) / 10).ToString();
//     }

// }

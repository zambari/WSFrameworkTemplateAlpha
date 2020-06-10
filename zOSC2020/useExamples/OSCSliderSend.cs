// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine.UI;
// using UnityEngine;
// using UnityOSC;
// using Z.OSC;
// // zOSC_1 use example
// [RequireComponent(typeof(Slider))]
// public class OSCSliderSend : MonoBehaviour
// {
//     // [Header("MODDED VERSION UWAGA")]
//     public string OSCAddress;
//     Slider slider;
//     public Text valueDisplayText;
//     public Text oscAddressDisplayText;
//     public bool sendAsInt;
//     public bool SendOSC = true;
//     public bool recieveOSC = true;
//     public bool printDebugs = false;
//     // bool antiFeedback;
//     // float antiFeedbackTime = 0.1f;
//     // float releaseFeedbackAfter;
//     float lastSentValue = -1;
//     void OnValidate()
//     {
//         printDebugs=false;
//         if (slider == null) slider = GetComponent<Slider>();
//         OSCAddress = OSCAddress.SanitizeOSCAddress();
//         if (valueDisplayText == null) valueDisplayText = GetComponentInChildren<Text>();
//         if (valueDisplayText != null) valueDisplayText.text = slider.value.ToString("0.00");
//         if (oscAddressDisplayText != null) oscAddressDisplayText.text = OSCAddress;
//     }

//     void Start()
//     {
//         slider = GetComponent<Slider>();
//         slider.onValueChanged.AddListener(OnNewSliderValue);

//         // zOSC_1.BindInt(this, OSCAddress, Recieveint);
//         if (recieveOSC)
//             zOSC_1.Bind(this, OSCAddress, RecieveFloat);
//     }

//     // void Recieveint(int i)
//     // {
//     //     slider.value = i;
//     //     Debug.Log(name + " " + OSCAddress + "  Recieved value " + i);
//     //     SetSliderValue(i);
//     // }
//     void RecieveFloat(float f)
//     {
//         if (recieveOSC) //&& Time.time > releaseFeedbackAfter
//         {
//             if (printDebugs) Debug.Log(name + " listatning osc at '" + OSCAddress + "' received " + f, gameObject);

//             lastSentValue = f;//workaround
//             slider.value = f;
//         }
//     }

//     void OnNewSliderValue(float f)
//     {
//         if (lastSentValue == f)
//             return;
//         lastSentValue = f;
//         if (valueDisplayText != null) valueDisplayText.text = slider.value.ToString("0.00");
//         if (SendOSC)// && lastSentValue != f)
//         {

//             if (sendAsInt)
//             {
//                 System.Int32 val = (System.Int32)f;//(f * (negative ? -1 : 1) * (sendmax ? System.Int32.MaxValue : 10000));
//                 OSCMessage msg = new OSCMessage(OSCAddress);
//                 // for (int i = 0; i < parameterRepeatCount; i++)
//                 msg.Append(val);
//                 Debug.Log("sent " + val);
//                 zOSC_1.BroadcastOSC(msg);
//             }
//             else
//             {
//                 zOSC_1.BroadcastOSC(OSCAddress, f);
//             }
//         }

//         if (valueDisplayText != null) valueDisplayText.text = slider.value.ToString("0.00");
//     }

// }

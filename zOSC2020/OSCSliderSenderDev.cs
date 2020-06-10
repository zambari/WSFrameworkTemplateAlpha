using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityOSC;
using Z.OSC;

[RequireComponent(typeof(Slider))]

public class OSCSliderSenderDev : MonoBehaviour
{

    public string OSCAddress;
    Slider slider;
    public bool sendAsInt;
    public bool printDebugs = false;
    // bool antiFeedback;
    // float antiFeedbackTime = 0.1f;
    // float releaseFeedbackAfter;
    float lastSentValue = -1;

    public ISendOSCSelector selector;
    ISendOSC sender;
    // public zOSCSender senderInstance;
    void Reset()
    {
        // senderInstance = GameObject.FindObjectOfType<zOSCSender>() as zOSCSender;
    }
    void OnValidate()
    {
        printDebugs = false;
        if (slider == null) slider = GetComponent<Slider>();
        OSCAddress = OSCAddress.SanitizeOSCAddress();
        selector.OnValidate(this);
        sender = selector.valueSource;

    }

    void Start()
    {
        slider = GetComponent<Slider>();

        slider.onValueChanged.AddListener(OnNewSliderValue);
        selector.OnValidate(this);
        sender = selector.valueSource;
        // zOSC_1.BindInt(this, OSCAddress, Recieveint);
    }

    // void Recieveint(int i)
    // {
    //     slider.value = i;
    //     Debug.Log(name + " " + OSCAddress + "  Recieved value " + i);
    //     SetSliderValue(i);
    // }

    void OnNewSliderValue(float f)
    {
        if (lastSentValue == f)
            return;
        lastSentValue = f;
        if (sender != null)
            sender.Send(OSCAddress, f);
    }

}
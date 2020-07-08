using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Z.Reflection;

public class ValueRemote
{

    ValueProxy link;
    public static Dictionary<int, ValueRemote> valueRemoteDict;
    Slider slider;
    Toggle toggle;
    Button button;
    InputField inputField;
    bool ignoreUI;

    public int memberId
    {
        get
        {
            return link.memberId;
        }
    }
    public void UpdateClientUI(string s)
    {
        if (inputField != null)
        {
            ignoreUI = true;
            inputField.text = s;
            ignoreUI = false;;
        }
    }
    public void UpdateClientUI(float f)
    {

        if (slider != null)
        {
            ignoreUI = true;
            if (f > slider.maxValue)
            {
                slider.maxValue = f * 1.1f; // 10% margin
            }
            else
            if (f < 0 && f < slider.minValue)
            {
                slider.minValue = f * 1.1f; // 10% margin
            }
            slider.value = f;
            ignoreUI = false;
        }
        else
        if (toggle != null)
        {
            ignoreUI = true;
            toggle.isOn = f > 0.5f;
            ignoreUI = false;
        }
        else
        if (inputField != null)
        {
            UpdateClientUI(f.ToString()); //temoprary
        }
        else

        {
            Debug.Log("dont know how to handle this");
        }
    }

    // 	public void UpdateClientUI(float f)
    // {

    // }
    public void BindInputField(InputField inputField)
    {
        inputField.text = link.lastStringValue;
        inputField.onEndEdit.AddListener((x) =>
            WSValueClient.RequestValueChange(memberId, x)
        );

        RegisterRemote(memberId, this);
    }
    public void BindSlider(SliderInfo sliderInfo)
    {
        this.slider = sliderInfo.slider;
        if (link.valueRange != Vector2.zero)
        {
            slider.minValue = link.valueRange.x;
            slider.maxValue = link.valueRange.y;
        }
        slider.value = link.lastValue;
        slider.onValueChanged.AddListener((x) => WSValueClient.RequestValueChange(memberId, x));
        slider.SetLabel(link.baseName);
        sliderInfo.PadRight(40);
        RegisterRemote(memberId, this);
    }

    public void BindToggle(Toggle toggle)
    {
        //  BindSlider(sliderInfo.slider);

        //public void BindSlider(Slider slider)
        //{
        this.toggle = toggle;
        toggle.isOn = link.lastValue >.5f;
        toggle.SetLabel(link.baseName);
        toggle.onValueChanged.AddListener((x) =>
        {
            if (!ignoreUI)
                WSValueClient.RequestValueChange(memberId, x?1 : 0);
        });

        RegisterRemote(memberId, this);
    }

    public void BindButton(Button button)
    {
        this.button = button;
        button.onClick.AddListener(() => WSValueClient.RequestValueChange(memberId, 1));
        button.SetLabel(link.baseName);
        // RegisterRemote(memberId, this);
    }

    // public void BindInputFieldString(InputField inputField)
    // {
    //     this.inputField = inputField;
    //     inputField.onEndEdit.AddListener(OnString);
    //     inputField.SetLabel(link.baseName);
    //     RegisterRemote(memberId, this);
    // }


// }
// void OnToggle(bool val)
// {
//     if (ignoreUI) return;
//     WSValueClient.RequestValueChange(memberId, val ? 1 : 0);
// }
// void OnSliderMove(float f)
// {
//     if (ignoreUI) return;
//     WSValueClient.RequestValueChange(memberId, f);
// }
public ValueRemote(ValueProxy link)
{
    this.link = link;
}
public static void RegisterRemote(int v, ValueRemote remote)
{
    if (valueRemoteDict == null)
        valueRemoteDict = new Dictionary<int, ValueRemote>();
    if (valueRemoteDict.ContainsKey(v))
    {
        Debug.Log(" warning, overriding entry " + v);
        valueRemoteDict[v] = remote;
    }
    else
        valueRemoteDict.Add(v, remote);
}

public static ValueRemote GetRemote(int v)
{
    ValueRemote remote = null;
    if (valueRemoteDict == null)
        valueRemoteDict = new Dictionary<int, ValueRemote>();
    if (valueRemoteDict.TryGetValue(v, out remote))
    {
        //	Debug.Log("remote found " + v);
    }
    else
    {
        Debug.Log("remote unknown " + v);
    }
    return remote;
}
}
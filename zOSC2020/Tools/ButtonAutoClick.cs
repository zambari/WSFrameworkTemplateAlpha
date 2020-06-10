using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonAutoClick : MonoBehaviour
{
    public int secondsToWait = 10;
    Text text;
    string basetext;
    IEnumerator Start()
    {
        text = GetComponentInChildren<Text>();
        Button button = GetComponent<Button>();
        basetext = text.text;
        while (secondsToWait >= 0)
        {
            yield return new WaitForSeconds(1);

            text.text = basetext + " (" + secondsToWait + ")";
            secondsToWait--;
        }
        button.onClick.Invoke();
        enabled = false;
    }
    void OnDisable()
    {
        StopAllCoroutines();
        if (text != null) text.text = basetext;

    }
}

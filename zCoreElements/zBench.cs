using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//v.02 experimetnal isprefab
//v.03 is selectedInEditor
//v.04 getipaddress, 2017 compatibility

public static class zBench
{
    static Dictionary<string, System.Diagnostics.Stopwatch> stopwatchdict;
    public static System.Diagnostics.Stopwatch GetStopWatch(string key)
    {
        if (stopwatchdict == null) stopwatchdict = new Dictionary<string, System.Diagnostics.Stopwatch>();
        System.Diagnostics.Stopwatch sw;
        if (stopwatchdict.TryGetValue(key, out sw))
        {
            return sw;
        }
        else
        {
            var newSw = new System.Diagnostics.Stopwatch();
            stopwatchdict.Add(key, newSw);
            return newSw;
        }

    }
    public static bool IsSelected(this Component src)
    {
#if UNITY_EDITOR
        if (src == null) return false;
        return UnityEditor.Selection.activeGameObject == src.gameObject;
#else
        return false;
#endif
    }

    public static bool PrefabModeIsActive(GameObject gameObject) //https://stackoverflow.com/questions/56155148/how-to-avoid-the-onvalidate-method-from-being-called-in-prefab-mode
    {
#if UNITY_EDITOR && UNITY_2018_3_OR_NEWER
        UnityEditor.Experimental.SceneManagement.PrefabStage prefabStage = UnityEditor.Experimental.SceneManagement.PrefabStageUtility.GetPrefabStage(gameObject);
        if (prefabStage != null)
            return true;
        if (UnityEditor.EditorUtility.IsPersistent(gameObject))
            return true;
#endif
        return false;

    }
    public static Dictionary<string, List<float>> callDictionary;

    public static void FlagIfKeepsFiring(string label, int maxCallsInTime = 10, float time = 2)
    {
        if (callDictionary == null) callDictionary = new Dictionary<string, List<float>>();
        List<float> thisList = null;
        if (!callDictionary.TryGetValue(label, out thisList))
        {

        }
        if (thisList == null) thisList = new List<float>();
        thisList.Add(Time.time);
        callDictionary.Add(label, thisList);
        while (thisList.Count > maxCallsInTime)
        {
            thisList.RemoveAt(0);
        }
        if (Time.time - thisList[0] < time)
        {
            Debug.Log("Warning More than " + maxCallsInTime + " calls labeled " + label + " were executed during last " + time);
        }
    }
    public static string GetIPAddress()
    {
        if (Application.isPlaying)
        {
            string strHostName = System.Net.Dns.GetHostName();
            #pragma warning disable 618
            System.Net.IPHostEntry iphostentry = System.Net.Dns.GetHostByName(strHostName);
            #pragma warning restore 618
            foreach (System.Net.IPAddress ipaddress in iphostentry.AddressList)
                if (ipaddress.GetAddressBytes().Length == 4)

                    return ipaddress.ToString();
        }
        return "x.x.x.x";
    }
    public static System.Diagnostics.Stopwatch Start(string key)
    {
        var sw = GetStopWatch(key);
        sw.Start();
        return sw;
    }

    public static System.Diagnostics.Stopwatch Resume(string key)
    {
        return Start(key);
    }
    public static int ElapsedMilliseconds(string key)
    {
        var sw = GetStopWatch(key);
        return (int) sw.ElapsedMilliseconds;
    }
    public static int ElapsedTicks(string key)
    {
        var sw = GetStopWatch(key);
        return (int) sw.ElapsedTicks;
    }
    public static int Pause(string key)
    {
        var sw = GetStopWatch(key);
        sw.Start();
        return (int) sw.ElapsedMilliseconds;
    }
    public static int End(string key)
    {
        var sw = GetStopWatch(key);
        sw.Stop();
        if (sw.ElapsedMilliseconds < 5)
            Debug.Log("Time between starting and finih of [" + key + "] was  " + sw.ElapsedMilliseconds + " ms (or " + sw.ElapsedTicks + " ticks)");
        else
            Debug.Log("Time between starting and finih of [" + key + "] was  " + sw.ElapsedMilliseconds + " ms (or " + sw.ElapsedTicks + " ticks)");
        stopwatchdict.Remove(key);
        return (int) sw.ElapsedMilliseconds;
    }
}
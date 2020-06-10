// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;

// public class OSCSettingsHelper : MonoBehaviour
// {
//     public InputField targetIP;
//     public InputField targetPort;
//     public InputField ownPort;
//     public Text ownIPText;
//     public void CopyOwn()
//     {
//         targetIP.text = ownIPText.text;

//     }
//     public void AddressMod(int dir)
//     {
//         string[] s = targetIP.text.Split('.');
//         if (s.Length != 4)
//         {
//             Debug.Log("does not look like ip"); return;
//         }
//         int v = 2;
//         System.Int32.TryParse(s[3], out v);
//         v += dir;
//         if (v < 0) v = 254;
//         if (v > 254) v = 0;
//         targetIP.text = s[0] + '.' + s[1] + '.' + s[2] + '.' + v;
//         Debug.Log("notched address " + dir);
//     }
//     public void AddressNotchDown()
//     {
//         AddressMod(-1);
//     }
//     public void SwapPorts()
//     {
//         var s = targetPort.text;
//         ownPort.text = targetPort.text;
//         ownPort.text = s;
//         Debug.Log("swapped ");
//     }
//     public void AddressNotchUP()
//     {
//         AddressMod(1);

//     }

//     IEnumerator Start()
//     {
//         yield return null;
//         zOSC_1 osc = zOSC_1.instance;
//         targetIP.text = osc.targetAddr;
//         targetPort.text = osc.targetPort.ToString();
//         ownPort.text = osc.defaultRecievePort.ToString();

//     }
//     public void LoadSaved()
//     {
//         string targetip = PlayerPrefs.GetString("IP", "127.0.0.1");
//         targetIP.text = targetip;
//         int ownport = PlayerPrefs.GetInt("OwnPort", 8899);
//         int targetPortNr = PlayerPrefs.GetInt("TargetPort", 7788);

//         targetPort.text = targetPortNr.ToString();
//         ownPort.text = ownport.ToString();

//         Debug.Log("settings restored from playerprefs");

//     }
//     int GetTargetPort()
//     {
//         int p = 9988;
//         System.Int32.TryParse(targetPort.text, out p);
//         return p;
//     }
//     int GetOwnPort()
//     {
//         int p = 8899;
//         System.Int32.TryParse(ownPort.text, out p);
//         return p;
//     }
//     public void Save()
//     {
//         PlayerPrefs.SetInt("OwnPort", GetOwnPort());
//         PlayerPrefs.SetInt("TargetPort", GetTargetPort());
//         PlayerPrefs.SetString("IP", targetIP.text);
//         Debug.Log("settings saved to playerprefs");
//     }
//     public void Apply()
//     {
//         Debug.Log("settings applied");
//         zOSC_1.SetLocalPort(GetOwnPort());
//         zOSC_1.SetTarget(targetIP.text, GetTargetPort());



//     }

// }

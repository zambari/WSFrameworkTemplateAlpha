using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
public class IPDisplay : MonoBehaviour
{
    System.Text.StringBuilder sb;
    Text text;
    void AddLine(string label, string value)
    {
        sb.Append(label + " : " + value + "\n");
    }
    void AddLine(string label, int value)
    {
        sb.Append(label + " : " + value + "\n");
    }

    void GetIPS()
    {
        string strHostName = Dns.GetHostName();
        IPHostEntry iphostentry = Dns.GetHostByName(strHostName);
        foreach (IPAddress ipaddress in iphostentry.AddressList)
        {
            if (ipaddress.GetAddressBytes().Length == 4)
            {
                var bytes = ipaddress.GetAddressBytes();
                if (bytes.Length == 4 && bytes[3] == 255) continue;
                sb.Append(ipaddress.ToString());
            }
        }
    }

    IEnumerator Start()
    {

        text = GetComponent<Text>();

        while (true)
        {
            sb = new System.Text.StringBuilder();
            GetIPS();
            text.text = sb.ToString();

            yield return new WaitForSeconds(40);

        }
    }
    /*   
*/


    /*
         public static string GetMac()
            {
    #if UNITY_STANDALONE_WIN || UNITY_EDITOR
                try
                {
                    NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
                    foreach (NetworkInterface adapter in nics)
                    {
    //                    var properties = adapter.GetIPProperties();
                        var tmp = adapter.GetPhysicalAddress();
                        if (tmp != null)
                        {
                            var regex = "(.{2})(.{2})(.{2})(.{2})(.{2})(.{2})";
                            var replace = "$1:$2:$3:$4:$5:$6";
                            return Regex.Replace(tmp.ToString().ToLower(), regex, replace);
                        }
                    }
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
                return "";
    #endif

    #if UNITY_ANDROID

            try
            {
                var fs = new FileStream(Path.Combine(Application.persistentDataPath, "ip"), FileMode.Open, FileAccess.Read);
                var sr = new StreamReader(fs);
                return sr.ReadToEnd().Trim();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
    #endif
              //  

            }
    */

}

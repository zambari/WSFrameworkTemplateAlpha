using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using zUI;

public class WSTargetAddress : MonoBehaviour
{
    [SerializeField] string _ipAddress = "127.0.0.1";
    public string ipAddress { get { return _ipAddress; } set { _ipAddress = value; } }
    public int port { get { return _port; } set { _port = value; } }
    public int _port = 4649;
    public string portString
    {
        get { return port.ToString(); }
        set
        {
            int newport = 0;
            if (System.Int32.TryParse(value, out newport))
            {
                port = newport;
            }
        }
    }

    [SerializeField]
    // [HideInInspector]
    RectTransform menuRect;
    public System.Action OnConnectRequested;

    public System.Action OnDisconnectRequested;
    public bool clientsShouldAutoConnect;
    public bool tryConnectingOnStart = true;
    public bool autoBuildMenu = true;
    void OnValidate()

    {
        if (!zBench.PrefabModeIsActive(gameObject))
            name = "WSTarget   " + ipAddress + " : " + port;
    }
    void Start()
    {
        if (tryConnectingOnStart) TryConnectingPingAndConnectRestIfSucceds();
        if (autoBuildMenu)
        {
            AddTopBar();
        }

    }

    [ExposeMethodInEditor]
    public void TryConnectingPingAndConnectRestIfSucceds()
    {
        StartCoroutine(PingTest());
        //	if (OnConnectRequested != null) OnConnectRequested();
    }

    IEnumerator PingTest()
    {
        var ping = GetComponent<WSPingClient>();
        int current = ping.recievedCount;
        ping.Connect();
        float t = Time.time;
        while (ping.recievedCount == current && Time.time - t < 5)
        {
            yield return null;
        }
        if (ping.recievedCount == current)
        {
            Debug.Log("no pings come trhought, aborting");
        }
        else
        {
            //    Debug.Log((ping.recievedCount - current) + " pings !!");
            BroadcastConnectionRequest();
        }
        yield return new WaitForSeconds(1);

    }

    [ExposeMethodInEditor]
    void AddTopBar()
    {

        if (menuRect == null)
        {
            menuRect = this.AddTopMenu();
        }

    }

    [ExposeMethodInEditor]
    public void BroadcastConnectionRequest()
    {
        if (OnConnectRequested != null) OnConnectRequested();
    }

    [ExposeMethodInEditor]
    public void BroadcastDiscConnectionRequest()
    {
        if (OnDisconnectRequested != null)
            OnDisconnectRequested();
        else
        {
            Debug.Log("no listeners");
        }
    }
}
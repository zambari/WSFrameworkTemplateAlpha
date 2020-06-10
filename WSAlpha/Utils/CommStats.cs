using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class CommStats
{
    [Header("Transmit")]
    [ReadOnly][SerializeField] protected int txBytesTotal;
    [ReadOnly][SerializeField] protected int txFrames;

    [Header("Recieve")]
    [ReadOnly][SerializeField] protected int rxFrames;
    [ReadOnly][SerializeField] protected int rxBytesTotal;

    [HideInInspector]
    [SerializeField] protected int rxBytesSinceTick;
    [HideInInspector]
    [SerializeField] int txBytesSinceTick;
    [Header("Rates")]
    [ReadOnly][SerializeField] protected int txBytesPerSecond;
    [ReadOnly][SerializeField] protected int rxBytesPerSecond;
    public void AddBytesRecieved(int rx)
    {
        rxBytesTotal += rx;
        rxBytesSinceTick += rx;
        rxFrames++;
    }
    public void AddBytesSent(int tx)
    {
        txBytesTotal += tx;
        txBytesSinceTick += tx;
        txFrames++;
    }
    public void UpdateAverages(float time)
    {
        if (time == 0) return;
        // Debug.Log("mesring "+txBytesTotal );
        txBytesPerSecond = Mathf.FloorToInt(txBytesSinceTick / time / 1024);
        txBytesSinceTick = 0;
        rxBytesPerSecond = Mathf.FloorToInt(rxBytesSinceTick / time / 1024);
        rxBytesSinceTick = 0;

    }
    public IEnumerator DataRateMeasurement()
    {
        int measureinterval = 3;
        while (true)
        {
            UpdateAverages(measureinterval);
            yield return new WaitForSeconds(measureinterval);
        }
    }

}
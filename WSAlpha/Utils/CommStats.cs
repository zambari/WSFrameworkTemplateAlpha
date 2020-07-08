using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class CommStats
{
    [Header("Transmit")]
    [ReadOnly] [SerializeField] public int txBytesTotal;
    [ReadOnly] [SerializeField] protected int txFrames;

    [Header("Recieve")]
    [ReadOnly] [SerializeField] protected int rxFrames;
    [ReadOnly] [SerializeField] public int rxBytesTotal;

    [HideInInspector]
    [SerializeField] protected int rxBytesSinceTick;
    [HideInInspector]
    [SerializeField] int txBytesSinceTick;
    [Header("Rates")]
    [ReadOnly] [SerializeField] protected int txBytesPerSecond;
    [ReadOnly] [SerializeField] protected float rxBytesPerSecond;
    [ReadOnly] [SerializeField] protected float rxMessagesPerSecondAverage;
    [ReadOnly] [SerializeField] [HideInInspector] protected int rxMessagesPerSecondTick;

    public bool printOnRecieve;
    public bool printOnSend;
    public void AddBytesRecieved(int rx)
    {
        rxBytesTotal += rx;
        rxBytesSinceTick += rx;
        rxFrames++;

        rxMessagesPerSecondTick++;
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
        txBytesPerSecond = Mathf.FloorToInt(txBytesSinceTick / time / 1);
        txBytesSinceTick = 0;
        rxBytesPerSecond = Mathf.FloorToInt(rxBytesSinceTick / time / 1);
        rxBytesSinceTick = 0;
        rxMessagesPerSecondAverage = rxMessagesPerSecondAverage * 0.7f + 0.3f * rxMessagesPerSecondTick / time;
        rxMessagesPerSecondTick = 0;

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
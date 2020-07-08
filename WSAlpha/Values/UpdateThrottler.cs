using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateThrottler
{

	float nextChange;
	float scanInterval;
	float minScanInterval = .1f;
	float maxScanInterval = .7f;
	public void NotifyChange()
	{
		scanInterval = minScanInterval;
		nextChange = Time.time + scanInterval;

	}

	public bool RequestScan()
	{
		if (Time.time < nextChange) return false;
		nextChange = Time.time + scanInterval;
		if (scanInterval < maxScanInterval)
			scanInterval += 0.02f;
		// Debug.Log("relaxing scn " + scanInterval);
		return true;

	}

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReleaseOnDestroy : MonoBehaviour
{
	WSComponentClient client;
	int componentID;
	ulong objectid;
	public void Init(WSComponentClient client, ulong objectid, int componentID)
	{
		this.client = client;

		this.objectid = objectid;
		this.componentID = componentID;
	}
	private void OnDestroy()
	{
if (client!=null) 
{
	client.Release(objectid, componentID);
}
	}
}
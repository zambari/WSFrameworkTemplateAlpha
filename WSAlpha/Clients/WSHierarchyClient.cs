using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;
//zbr 2020

public class WSHierarchyClient : WSClientBase
{

    protected override void OnMessageDequeue(MessageEventArgs s)
    {
     		DebugClient(" got :" + s.Data);
		
		var obj= JsonUtility.FromJson<WSFrameworkMessage>(s.Data);
		if (obj.type!="hierarchyResponse")
		return;
		var req=JsonUtility.FromJson<WSHierarchyResponse>(s.Data);
	//	Debug.Log("req.root"+req.root);

		var response = new WSHierarchyResponse();
		{

		//	var allRoots=new 
		}
    }
   
}
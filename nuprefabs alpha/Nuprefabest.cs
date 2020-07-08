using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using zUI;
public class Nuprefabest : MonoBehaviour
{

	// Use this for initialization
	[ExposeMethodInEditor]
	void TEstPrefabs()
	{
		var helper = UIPrefabProvider.GetPrefabs(this);
		if (helper == null)
		{
			Debug.Log("failed getting helper");
		}
		else
		{
			var subpanel1 = helper.GetPanel("hello");
			var helper1 = UIPrefabProvider.GetPrefabs(this, subpanel1);

			var subpanel2 = helper.GetPanel("hello",true);
			var helper2 = UIPrefabProvider.GetPrefabs(this, subpanel2);



			helper.GetButton("hello");
			helper.GetSlider("slider");
			helper.GetToggle("toggle");
			helper.GetLabel("label");
			helper.GetInputField("inputf");
			

			helper1.GetButton("hello");
			helper1.GetSlider("slider");
			helper1.GetToggle("toggle");
			helper1.GetLabel("label");
			helper1.GetInputField("inputf");

			helper2.GetButton("hello");
			helper2.GetSlider("slider");
			helper2.GetToggle("toggle");
			helper2.GetLabel("label");
			helper2.GetInputField("inputf");

		}

	}

}
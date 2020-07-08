using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using zUI;
#if UNITY_EDITOR
using UnityEditor;
public class UIBuildCommands : EditorWindow
{

	[MenuItem("CONTEXT/RectTransform/Add scrollrect ")]
	static void AddWithscroll(MenuCommand command)
	{

		AddWithscroll((RectTransform) command.context);

	}

	[MenuItem("CONTEXT/RectTransform/Add both - Convert To Panel ")]
	static void AddFrames(MenuCommand command)
	{

		AddFrames((RectTransform) command.context);

	}

	[MenuItem("CONTEXT/RectTransform/Add top ")]
	static void AddTop(MenuCommand command)
	{

		AddTop((RectTransform) command.context);

	}
	static void AddFrames(RectTransform target)
	{
		Undo.RecordObject(target, "Panels");
		PanelInfo info = PanelInfo.ConvertToPanelNonChild(target, true);
		//info.mainRect.AddLayoutElementFromExistingOrIgnoreIfExists(300, 300, 0.1f, 0.5f);
		info.AddRectForContent();
		//  info.ConvertContentForSCrollVEiw();
		info.content.AddVerticalLayout(); //.AddLeftPaddToLayout(leftPad);
		info.SetLabel(target.name);
	}
	static void AddTop(RectTransform target)
	{
		Undo.RecordObject(target, "Panels");
		PanelInfo info = PanelInfo.ConvertToPanelNonChild(target, false);
//		info.mainRect.AddLayoutElement(300, 300, 0.1f, 0.5f);
		info.AddRectForContent();
		info.content.AddVerticalLayout();
		info.SetLabel(target.name);
	}

	static void AddWithscroll(RectTransform target)
	{
		Undo.RecordObject(target, "Panels");
		PanelInfo info = PanelInfo.ConvertToPanelNonChild(target, true);

		info.mainRect.AddLayoutElementFromExistingOrIgnoreIfExists();
		info.AddRectForContent();
		info.ConvertContentForSCrollVEiw();
		info.content.AddVerticalLayout(); //.AddLeftPaddToLayout(leftPad);
		info.SetLabel(target.name);
	}
}
#endif
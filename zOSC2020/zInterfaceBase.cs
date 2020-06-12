using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Z
{
	[System.Obsolete]
	public interface UnityBaseInterface: zInterfaceBase {}
	public interface zInterfaceBase
	{
		GameObject gameObject { get; }
		string name { get; }
		Transform transform { get; }
	}
}
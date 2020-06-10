using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Z
{
	public interface UnityBaseInterface
	{
		GameObject gameObject { get; }
		string name { get; }
		Transform transform { get; }
	}
}
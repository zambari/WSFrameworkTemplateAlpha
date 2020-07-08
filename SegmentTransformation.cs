using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Z;

public class SegmentTransformation : MonoBehaviour
{

	[Range(0, 4)]
	[SerializeField] int _quadSegmentTick;
		public int quadSegmentTick
	{
		get { return _quadSegmentTick; }
		set
		{
			_quadSegmentTick = value;
			SetRotation();

		}
	}
	[Range(-90, 90)]
	[SerializeField] float _fineRotation;
	public float fineRotation { get { return _fineRotation; } set { _fineRotation = value; SetRotation(); } }


	[Range(.01f, 4)]
	[SerializeField] float _scale = 1;
	public float scale { get { return _scale; } set { _scale = value; SetRotation(); } }

	[Range(.01f, 16)]
	[SerializeField] float _deptthOFfset = 1;
	public float depthOffset { get { return _deptthOFfset; } set { _deptthOFfset = value; SetRotation(); } }

	[Range(-5, 5)]
	[SerializeField] float _height = 0;
	public float height { get { return _height; } set { _height = value; SetRotation(); } }

	[Range(-25, 25)]
	[SerializeField] float _offsetHoriz = 0;
	public float offsetHoriz { get { return _offsetHoriz; } set { _offsetHoriz = value; SetRotation(); } }
	public bool disablemovemnetOnStart = true;

	void SetRotation()
	{
		transform.localRotation = Quaternion.Euler(0, 90 * _quadSegmentTick + fineRotation, 0);
		var s = scale * scale;
		transform.localScale = new Vector3(s, s, s);
		transform.localPosition = new Vector3(_offsetHoriz, height*3, -depthOffset);
	}
	private void OnValidate()
	{
		SetRotation();
	}

	[ExposeMethodInEditor]
	void ResetLocalrotations()
	{
		StartCoroutine(Str());
	}
	IEnumerator Str()
	{
		var cg = GetComponentsInChildren<Transform>();
		foreach (var c in cg)
		{
			c.localRotation = Quaternion.identity;
			yield return null;
			yield return null;
		}
	}

	[ExposeMethodInEditor]
	void DisableMovementSources()

	{
		var brs = Resources.FindObjectsOfTypeAll(typeof(BrownianMotionZ)) as BrownianMotionZ[];
		var simrots = Resources.FindObjectsOfTypeAll(typeof(SimpleRotate)) as SimpleRotate[];
		foreach (var b in brs)
		{
			if (b.GetComponentInParent<SegmentTransformation>() == this) b.enabled = false;
		}
		foreach (var s in simrots)
		{
			if (s.GetComponentInParent<SegmentTransformation>() == this) s.enabled = false;
		}
	}
	void Start()
	{
		if (disablemovemnetOnStart) DisableMovementSources();
		if (zPath.Exists(zPath.AppRootPath("1.txt"))) quadSegmentTick = 1;
		if (zPath.Exists(zPath.AppRootPath("2.txt"))) quadSegmentTick = 2;
		if (zPath.Exists(zPath.AppRootPath("3.txt"))) quadSegmentTick = 3;
		if (zPath.Exists(zPath.AppRootPath("4.txt"))) quadSegmentTick = 4;
	}

}
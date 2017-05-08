// ---------------------------------------
// Brief : ギミックの拡大縮小
// 
// Date  : 2017/05/08
// 
// Author: Y.Watanabe
// ---------------------------------------

using UnityEngine;
using DG.Tweening;

public class GimmickScaler : GimmickTarget
{
	// 制限をかける
	[System.Serializable]
	class Constraints
	{
		public bool x, y, z;
	};

	[SerializeField]
	Constraints m_freeze;

	public override Tween GimmickTween (Transform gimmick)
	{
		Vector3 vec = transform.localScale;

		if (m_freeze.x) { vec.x = gimmick.transform.localScale.x; }
		if (m_freeze.y) { vec.y = gimmick.transform.localScale.y; }
		if (m_freeze.z) { vec.z = gimmick.transform.localScale.z; }

		return gimmick
			.DOScale(vec, Duration)
			.SetEase(EaseType)
			.SetDelay(Delay);
	}
}

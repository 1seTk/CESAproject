// ---------------------------------------
// Brief : ギミックの回転
// 
// Date  : 2017/05/02
// 
// Author: Y.Watanabe
// ---------------------------------------

using UniRx;
using UnityEngine;
using System.Collections;
using DG.Tweening;

public class GimmickRotater : GimmickTarget
{
	[SerializeField, Tooltip("ローカル座標で回転するか")]
	private bool m_isLocal = false;

	public override Tween GimmickTween(Transform gimmick)
	{
		if(m_isLocal)
		{
			return gimmick
				.DOLocalRotateQuaternion(transform.localRotation, Duration)
				.SetEase(EaseType)
				.SetDelay(Delay);
		}
		else
		{
			return gimmick
				.DORotate(transform.rotation.eulerAngles, Duration)
				.SetEase(EaseType)
				.SetDelay(Delay);
		}
	}
}

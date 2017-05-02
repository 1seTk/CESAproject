// ---------------------------------------
// Brief : ギミックの移動
// 
// Date  : 2017/05/02
// 
// Author: Y.Watanabe
// ---------------------------------------

using UniRx;
using UnityEngine;
using System.Collections;
using DG.Tweening;

public class GimmickMover : GimmickTarget
{
	public override Tween GimmickTween (Transform gimmick)
	{
		return gimmick
			.DOLocalMove(transform.localPosition, Duration)
			.SetEase(EaseType)
			.SetDelay(Delay);
	}
}

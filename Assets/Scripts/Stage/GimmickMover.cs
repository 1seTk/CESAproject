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
	[SerializeField, Tooltip("初期座標へ移動するか")]
	public bool m_isReset = false;

	// コンストラクタ
	public void Converse (GimmickTarget target)
	{
		m_easeType = target.EaseType;
		m_duration = target.Duration;
		m_delay = target.Delay;
	}

	public override Tween GimmickTween (Transform gimmick)
	{
		if(m_isReset)
		{
			return gimmick
				// 初期座標を取得して戻す(もっといい方法ありそう…)
				.DOMove(transform.GetComponentInParent<GimmickCore>().StartPosition, Duration)
				.SetEase(EaseType)
				.SetDelay(Delay);
		}
		else
		{
			return gimmick
				.DOLocalMove(transform.localPosition, Duration)
				.SetEase(EaseType)
				.SetDelay(Delay);
		}
	}
}

// ---------------------------------------
// Brief : ギミックの移動ターゲット
// 
// Date  : 2017/04/28
// 
// Author: Y.Watanabe
// ---------------------------------------

using UniRx;
using UnityEngine;
using System.Collections;
using DG.Tweening;

public class GimmickTarget : MonoBehaviour
{
	[SerializeField, Tooltip("イージング方法")]
	protected Ease m_easeType;

	public Ease EaseType
	{
		get { return m_easeType; }
	}

	[SerializeField, Range(0, 30), Tooltip("移動にかかる時間")]
	protected float m_duration;

	public float Duration
	{
		get { return m_duration; }
	}

	[SerializeField, Range(0, 30), Tooltip("ディレイ(遅延する時間)")]
	protected float m_delay = 0;

	public float Delay
	{
		get { return m_delay; }
	}

	public virtual Tween GimmickTween(Transform gimmick) { return null; }

	/// <summary>
	/// 複製用関数
	/// </summary>
	/// <returns></returns>
	public virtual object Clone()
	{
		//GimmickTarget target = new GimmickTarget();
		//target.m_easeType = m_easeType;
		//target.m_duration = m_duration;
		//target.m_delay = m_delay;

		//return target;
		return MemberwiseClone();
	}
}

// ---------------------------------------
// Brief : ギミックの移動
// 
// Date  : 2017/04/24
// 
// Author: Y.Watanabe
// ---------------------------------------

using UniRx;
using UnityEngine;
using System.Collections;
using DG.Tweening;

public class GimmickMover : MonoBehaviour
{
	[SerializeField]
	private Vector3 m_targetPosition;

	[SerializeField, Range(0, 10)]
	private float m_duration;

	[SerializeField]
	private Ease m_easeType;

	/// <summary> 
	/// 更新前処理
	/// </summary>
	void Start ()
	{
		transform.DOMove(m_targetPosition, m_duration)
			.SetEase(m_easeType)
			.SetLoops(-1, LoopType.Yoyo);
	}
}

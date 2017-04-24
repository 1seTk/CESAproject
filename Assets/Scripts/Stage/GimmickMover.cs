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
	private float m_targetPosition;

	// private 

	/// <summary> 
	/// 更新前処理
	/// </summary>
	void Start ()
	{
		// transform.DOLocalMoveX(m_targetPosition, )
	}
}

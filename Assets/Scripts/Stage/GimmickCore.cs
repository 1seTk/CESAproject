// ---------------------------------------
// Brief : ギミックのコア部分
// 
// Date  : 2017/04/28
// 
// Author: Y.Watanabe
// ---------------------------------------

using UniRx;
using UniRx.Triggers;
using UnityEngine;
using System.Collections;
using DG.Tweening;

public class GimmickCore : MonoBehaviour
{
	[SerializeField, Tooltip("ループさせるかどうか")]
	private bool isLoop;

	// 移動先ターゲットリスト
	private GimmickTarget[] m_targets;

	// 次のターゲット
	private GimmickTarget m_nextTarget;

	// 現在のターゲット番号
	private int m_currentTarget = 1;

	// ギミックへの参照
	private Transform m_gimmick;

	// ギミックの初期座標
	private Vector3 m_startPosition;

	/// <summary> 
	/// 更新前処理
	/// </summary>
	void Start ()
	{
		// ギミックへの参照を取得(子のコライダーを持ったゲームオブジェクト)
		m_gimmick = GetComponentInChildren<Collider>().transform;

		// ギミックの初期座標を取得
		m_startPosition = m_gimmick.position;

		// 子からターゲット情報を取得
		m_targets = GetComponentsInChildren<GimmickTarget>();

		// コルーチンの作動
		StartCoroutine(WaitDoMove());
	}

	IEnumerator WaitDoMove()
	{
		// ターゲット情報をリストから取得
		m_nextTarget = m_targets[m_currentTarget];

		Tween tween;

		// ギミック自身が移動対象の場合の例外
		if (m_currentTarget == 0)
		{
			// ギミックの初期座標に向かって移動する(ワールド座標)
			tween = m_gimmick
				.DOMove(m_startPosition, m_nextTarget.Duration)
				.SetEase(m_nextTarget.EaseType)
				.SetDelay(m_nextTarget.Delay);
		}
		else
		{
			// 次のターゲットに向かって移動する
			tween = m_gimmick
				.DOLocalMove(m_nextTarget.transform.localPosition, m_nextTarget.Duration)
				.SetEase(m_nextTarget.EaseType)
				.SetDelay(m_nextTarget.Delay);
		}

		// 移動時間だけ待機する
		yield return new WaitForSeconds(tween.Duration() + tween.Delay());

		// ターゲットを次に進める
		m_currentTarget++;

		// 次のターゲットの番号が全ターゲット数を越えていたら
		if (m_currentTarget >= m_targets.Length)
		{
			// ループフラグが立っていたら
			if (isLoop == true)
			{
				// ターゲット番号をはじめに戻す
				m_currentTarget = 0;
				// コルーチンの再帰
				StartCoroutine(WaitDoMove());
			}
		}
		else
		{
			// コルーチンの再帰
			StartCoroutine(WaitDoMove());
		}
	}
}

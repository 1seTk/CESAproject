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
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;

public class GimmickCore : MonoBehaviour
{
	[SerializeField, Tooltip("ループさせるかどうか")]
	private bool isLoop;

	// 移動先ターゲットリスト
	private List<Transform> m_targets = new List<Transform>();
	//private Transform[] m_targets;

	// 次のターゲット
	private GimmickTarget[] m_nextTarget;

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

		// ターゲットへの登録処理 ======
		// 自身を登録
		m_targets.Add(transform);

		// 子からターゲット情報を取得
		var t = m_gimmick.GetComponentsInChildren<Transform>();

		// 子要素があればそれらを登録する
		if(t.Length > 0)
		{
			// ギミック本体以外を登録
			for (int i = 1; i < t.Length; i++)
			{
				m_targets.Add(t[i]);
			}

			// コルーチンの作動
			StartCoroutine(WaitDoMove());
		}
	}

	IEnumerator WaitDoMove()
	{
		// ターゲット情報をリストから取得
		m_nextTarget = m_targets[m_currentTarget].GetComponents<GimmickTarget>();

		// 待機時間
		float waitTime = 0.0f;

		// 次のターゲットオブジェクトのTweenを全て実行する
		foreach (var item in m_nextTarget)
		{
			Tween tween = item.GimmickTween(m_gimmick);
			float t = tween.Duration() + tween.Delay();
			if (t > waitTime)
				waitTime = tween.Duration() + tween.Delay();
		}

		// 移動時間だけ待機する
		yield return new WaitForSeconds(waitTime);

		Debug.Log("move end");

		// ターゲットを次に進める
		m_currentTarget++;

		// 次のターゲットの番号が全ターゲット数を越えていたら
		if (m_currentTarget >= m_targets.Count)
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

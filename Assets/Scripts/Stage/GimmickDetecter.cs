// ---------------------------------------
// Brief : 判定付きギミック
// 
// Date  : 2017/05/08
// 
// Author: Y.Watanabe
// ---------------------------------------

using UniRx;
using UniRx.Triggers;
using UnityEngine;
using System.Collections;
using DG.Tweening;

public class GimmickDetecter : MonoBehaviour
{
	private bool m_isEnter = false;

	public BoolReactiveProperty IsEnterRP = new BoolReactiveProperty(false);

	private GimmickCore[] m_cores;

	/// <summary> 
	/// 更新前処理
	/// </summary>
	void Start ()
	{
		// コアの参照
		m_cores = GetComponentsInChildren<GimmickCore>();

		SwitchActive();

		// 衝突した時
		this.OnTriggerEnterAsObservable()
			.Subscribe(_ =>
			{
				Debug.Log("player enter");
				m_isEnter = true;
			});

		// 離れたとき
		this.OnTriggerExitAsObservable()
			.Subscribe(_ =>
			{
				Debug.Log("player exit");
				m_isEnter = false;
			});

		// 衝突情報の更新と通知
		this.UpdateAsObservable()
			.Select(_ => m_isEnter)
			.Subscribe(x => IsEnterRP.SetValueAndForceNotify(x));

		// アクティブの切り替え
		IsEnterRP
			.DistinctUntilChanged()
			.Subscribe(x =>
			{
				Debug.Log("aa");
				SwitchActive();
			});
	}

	void SwitchActive()
	{
		m_cores[0].GetComponentInChildren<Renderer>().enabled = m_isEnter;
		m_cores[1].GetComponentInChildren<Renderer>().enabled = !m_isEnter;

		m_cores[0].enabled = m_isEnter;
		m_cores[1].enabled = !m_isEnter;

		m_cores[0].IsLoop = m_isEnter;
		m_cores[1].IsLoop = !m_isEnter;
	}
}

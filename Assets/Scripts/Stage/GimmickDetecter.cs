// ---------------------------------------
// Brief : ギミックの切り替え
// 
// Date  : 2017/05/08
// 
// Author: Y.Watanabe
// ---------------------------------------

using UniRx;
using UniRx.Triggers;
using UnityEngine;
using System;

public class GimmickDetecter : MonoBehaviour
{
	public bool m_isEnter = false;

	[HideInInspector]
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

		// 衝突情報の更新と通知
		this.UpdateAsObservable()
			.Select(_ => m_isEnter)
			.Subscribe(x => IsEnterRP.SetValueAndForceNotify(x));

		// アクティブの切り替え
		IsEnterRP
			.DistinctUntilChanged()
			.Subscribe(x =>
			{
				SwitchActive();
			});
	}

	void SwitchActive()
	{

		m_cores[0].GetComponentInChildren<Renderer>().enabled = m_isEnter;
		m_cores[1].GetComponentInChildren<Renderer>().enabled = !m_isEnter;

		m_cores[0].GetComponentInChildren<Collider>().enabled = m_isEnter;
		m_cores[1].GetComponentInChildren<Collider>().enabled = !m_isEnter;

		// 非アクティブになるオブジェクトの座標をアクティブになるオブジェクトの座標にコピー
		int foo = Convert.ToInt32(!m_isEnter);
		int bar = Convert.ToInt32(m_isEnter);

		m_cores[foo].transform.GetChild(0).localPosition = m_cores[bar].transform.GetChild(0).localPosition;
		m_cores[foo].transform.GetChild(0).localRotation = m_cores[bar].transform.GetChild(0).localRotation;
		m_cores[foo].transform.GetChild(0).localScale = m_cores[bar].transform.GetChild(0).localScale;

		m_cores[0].enabled = m_isEnter;
		m_cores[1].enabled = !m_isEnter;

	}
}

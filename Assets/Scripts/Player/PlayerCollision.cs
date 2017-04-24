// ---------------------------------------
// Brief : プレイヤーの当たり判定
// 
// Date  : 2014/04/24
// 
// Author: Y.Watanabe
// ---------------------------------------

using UniRx;
using UniRx.Triggers;
using UnityEngine;
using System.Collections;

public class PlayerCollision : MonoBehaviour
{
	private Collider[] m_colliders;


	// はさまれているか
	private int m_isInterPose;

	/// <summary> 
	/// 更新前処理
	/// </summary>
	void Start ()
	{
		// Colliderの参照
		m_colliders = GetComponentsInChildren<Collider>();

		// RigitBodyのスリープ解除用
		var rb = GetComponentInParent<Rigidbody>();

		this.UpdateAsObservable()
			.Subscribe(_ => rb.WakeUp());

		//this.UpdateAsObservable()
		//	.Where(Phi)

		foreach (var item in m_colliders)
		{
			item.OnTriggerEnterAsObservable()
				.Where(col => col != transform.root.GetComponent<Collider>())
				.Subscribe(_ => {
					m_isInterPose++;
					Debug.Log(_.transform.name);
				 });

			item.OnTriggerExitAsObservable()
				.Where(col => col != transform.root.GetComponent<Collider>())
				.Subscribe(_ => {
					m_isInterPose--;
					Debug.Log(_.transform.name);
				});

		}

		this.UpdateAsObservable()
			.Subscribe(_ => Debug.Log(m_isInterPose));
	}
}

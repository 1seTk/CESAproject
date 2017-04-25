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
using System.Linq;
using System.Collections.Generic;

public class PlayerCollision : MonoBehaviour
{
	private List<Collider> m_colliders;

	// 衝突方向
	private bool[] m_hitDirections = new bool[4] { false, false, false, false };

	/// <summary> 
	/// 更新前処理
	/// </summary>
	void Start ()
	{
		// 自身以外のColliderを参照
		var c = GetComponentsInChildren<Collider>();
		foreach (var item in c)
		{
			if(item.transform.GetInstanceID() != transform.GetInstanceID())
			{
				m_colliders.Add(item);
			}
		}

		// RigitBodyのスリープ解除用
		var rb = GetComponentInParent<Rigidbody>();

		this.UpdateAsObservable()
			.Subscribe(_ => rb.WakeUp());

		// はさまれた判定を取る
		foreach (var item in m_colliders.Select((v, i) => new { Value = v, Index = i}))
		{
			item.Value.OnTriggerEnterAsObservable()
				.Where(col => col != transform.root.GetComponent<Collider>())
				.Subscribe(_ => {
					m_hitDirections[item.Index] = true;
					Debug.Log(_.transform.name);
				 });

			item.Value.OnTriggerExitAsObservable()
				.Where(col => col != transform.root.GetComponent<Collider>())
				.Subscribe(_ => {
					m_hitDirections[item.Index] = false;
					Debug.Log(_.transform.name);
				});

		}

		this.UpdateAsObservable()
			.Where(_ => m_hitDirections[0] == true)
			.Where(_ => m_hitDirections[1] == true)
			.Subscribe(_ => {
				Destroy(transform.root.gameObject);
			 });

		this.UpdateAsObservable()
			.Where(_ => m_hitDirections[2] == true)
			.Where(_ => m_hitDirections[3] == true)
			.Subscribe(_ => Destroy(transform.root.gameObject));

	}
}

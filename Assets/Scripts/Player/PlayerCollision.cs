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
	[SerializeField]
	private List<Collider> m_colliders;

	// 衝突方向
	private bool[] m_hitDirections = new bool[4] { false, false, false, false };

	// 衝突距離
	[SerializeField, Range(0, 10)]
	private float m_hitDistance;

	private ReactiveProperty<bool> hitRP = new ReactiveProperty<bool>();

	// 衝突しているか
	public IReactiveProperty<bool> Hit
	{
		get { return hitRP; }
	}


	/// <summary> 
	/// 更新前処理
	/// </summary>
	void Start ()
	{
		// 自身以外のColliderを参照
		var c = GetComponentsInChildren<Collider>();
		foreach (var item in c)
		{
			if (item.transform.GetInstanceID() != transform.GetInstanceID())
			{
				m_colliders.Add(item);
			}
		}

		// RigitBodyのスリープ解除用
		var rb = GetComponentInParent<Rigidbody>();

		this.UpdateAsObservable()
			.Subscribe(_ => rb.WakeUp());

		// はさまれた判定を取る
		foreach (var item in m_colliders.Select((v, i) => new { Value = v, Index = i }))
		{
			item.Value.OnTriggerEnterAsObservable()
				.Where(col => col != transform.root.GetComponent<Collider>())
				.Subscribe(_ =>
				{
					m_hitDirections[item.Index] = true;
					Debug.Log(_.transform.name);
				});

			item.Value.OnTriggerExitAsObservable()
				.Where(col => col != transform.root.GetComponent<Collider>())
				.Subscribe(_ =>
				{
					m_hitDirections[item.Index] = false;
					Debug.Log(_.transform.name);
				});

		}

		// 左右のはさまれた判定
		this.UpdateAsObservable()
			.Where(_ => m_hitDirections[0] == true)
			.Where(_ => m_hitDirections[1] == true)
			.Subscribe(_ =>
			{
				Destroy(transform.root.gameObject);
			});

		// 上下のはさまれた判定
		this.UpdateAsObservable()
			.Where(_ => m_hitDirections[2] == true)
			.Where(_ => m_hitDirections[3] == true)
			.Subscribe(_ => Destroy(transform.root.gameObject));

		// 衝突情報と通知を飛ばす
		this.UpdateAsObservable()
			.Select(_ => Physics.Raycast(transform.position, transform.forward, m_hitDistance))
			.Subscribe(x => hitRP.SetValueAndForceNotify(x));
	}
}

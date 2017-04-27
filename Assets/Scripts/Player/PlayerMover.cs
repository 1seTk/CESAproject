﻿// ---------------------------------------
// Brief : プレイヤーの移動
// 
// Date  : 2017/04/24
// 
// Author: Y.Watanabe
// ---------------------------------------

using UniRx;
using UniRx.Triggers;
using UnityEngine;
using System.Collections;

public class PlayerMover : MonoBehaviour
{
	[SerializeField, Range(0, 10)]
	private float m_speed;

	private bool m_canMove = true;

	private ReactiveProperty<bool> isMovingRP = new ReactiveProperty<bool>();

	// 移動しているか
	public IReactiveProperty<bool> IsMoving
	{
		get { return isMovingRP; }
	}

	/// <summary> 
	/// 更新前処理
	/// </summary>
	void Start ()
	{
		var col = GetComponent<PlayerCollision>();

		// 移動入力
		this.UpdateAsObservable()
			.Select(_ => Input.GetKey(KeyCode.Space))
			.Subscribe(x =>
			{
				isMovingRP.SetValueAndForceNotify(x);
			});

		// 移動処理
		this.UpdateAsObservable()
			.Where(_ => IsMoving.Value == true)
			.Where(_ => m_canMove == true)
			.Subscribe(x =>
			{
				Debug.Log("Pressed Space");
				transform.position += new Vector3(0, 0, m_speed) * Time.deltaTime;
			});

		// 衝突状態によって移動を制限する
		this.UpdateAsObservable()
			.Subscribe(_ =>	m_canMove = !col.IsHit.Value);
	}
}

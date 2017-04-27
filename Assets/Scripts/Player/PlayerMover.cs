// ---------------------------------------
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

	/// <summary> 
	/// 更新前処理
	/// </summary>
	void Start ()
	{
		var col = GetComponent<PlayerCollision>();

		// 移動入力
		this.UpdateAsObservable()
			.Where(_ => Input.GetKey(KeyCode.Space))
			.Where(_ => m_canMove == true)
			.Subscribe(_ =>
			{
				Debug.Log("Pressed Space");
				transform.position += new Vector3(0, 0, m_speed) * Time.deltaTime;
			});

		// 衝突状態によって移動を制限する
		this.UpdateAsObservable()
			.Subscribe(_ =>	m_canMove = !col.Hit.Value);
	}
}

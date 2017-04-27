// ---------------------------------------
// Brief : プレイヤーとオブジェクトとの摩擦
// 
// Date  : 2017/04/27
// 
// Author: Y.Watanabe
// ---------------------------------------

using UniRx;
using UnityEngine;
using System.Collections;

public class PlayerFriction : MonoBehaviour
{
	/// <summary> 
	/// 更新前処理
	/// </summary>
	void Start ()
	{
		var col = GetComponent<PlayerCollision>();

		// プレイヤーとオブジェクトが衝突した時
		col.Hit
			.Where(x => x == true)
			.DistinctUntilChanged()
			.Subscribe(_ =>
			{
				Debug.Log("Player Hit Object");

			});

		// プレイヤーとオブジェクトが離れた時
		col.Hit
			.Where(x => x == false)
			// 何かのオブジェクトの子になっているか
			.Where(_ => transform.root.GetInstanceID() == transform.GetInstanceID())
			.DistinctUntilChanged()
			.Subscribe(_ =>
			{
				Debug.Log("Player Exit Object");

			});
	}
}

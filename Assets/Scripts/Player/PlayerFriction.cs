// ---------------------------------------
// Brief : プレイヤーの摩擦
// 
// Date  : 2017/05/26
// 
// Author: Y.Watanabe
// ---------------------------------------

using UniRx;
using UniRx.Triggers;
using UnityEngine;
using System.Collections;

public class PlayerFriction : MonoBehaviour
{
	// 衝突距離
	[SerializeField, Range(0, 10)]
	private float m_hitDistance;

	[SerializeField]
	private LayerMask m_ignoreMask;

	void Start ()
	{
		RaycastHit hit = new RaycastHit();

		// 四方にレイを飛ばす

		// レイにヒットしたオブジェクトを方向のリストに
		// 方向のリストないでの合計移動量を計算
		// 計算結果をプレイヤーに反映

		// or

		// ギミックに速度を持たせる
		// レイにヒットしたオブジェクトの速度を合計
		// 計算結果をプレイヤーに反映

		// レイにヒットしたオブジェクトに追従する
		// ただし、前面のレイはXY方向
		// 左右面はYZ方面のみに追従する
		// Y方向のマイナスは0に戻す
		// XZ方向は少しづつ減らす

		//// 衝突情報と通知を飛ばす
		//this.UpdateAsObservable()
		//	.Select(_ => Physics.Raycast(transform.position, transform.forward, out hit, m_hitDistance))
		//	.Subscribe(x => {
		//		isHitRP.SetValueAndForceNotify(x);
		//		m_hitObject = hit.transform;
		//		Debug.Log(m_hitObject.name);
		//	});

		// 衝突情報と通知を飛ばす
		this.UpdateAsObservable()
			.Where(_ => Physics.Raycast(transform.position, -transform.up, out hit, m_hitDistance, ~m_ignoreMask) == true)
			.Subscribe(x => {
				//isHitRP.SetValueAndForceNotify(x);
				//m_hitObject = hit.transform;
				//Debug.Log(m_hitObject.name);
				Debug.Log("hit");
			});


		// デバッグRay描画
		this.UpdateAsObservable()
			.Subscribe(_ => Debug.DrawRay(transform.position, -transform.up, Color.red, m_hitDistance));


		// 衝突していたら子にする、離れたら子にしない
	}
}

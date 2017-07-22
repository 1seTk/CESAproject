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

	RaycastHit hit = new RaycastHit();

	private Vector3 m_scale;

	private Vector3 m_defaultScale;

	void Start ()
	{

		var cg = GetComponent<CheckGround>();

		m_scale = transform.localScale;
		m_defaultScale = transform.lossyScale;

		{
			Vector3 lossScale = transform.lossyScale;
			Vector3 localScale = transform.localScale;
			transform.localScale = new Vector3(
					localScale.x / lossScale.x * m_defaultScale.x,
					localScale.y / lossScale.y * m_defaultScale.y,
					localScale.z / lossScale.z * m_defaultScale.z
			);

		}

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
			//.Where(_ => Physics.Raycast(transform.position, -transform.up, out hit, m_hitDistance, ~m_ignoreMask) == true)
			.Subscribe(x => {
				// Rayを飛ばす
				// var h = Physics.BoxCast(transform.position, Vector3.one * 0.5f, -transform.up, out hit, Quaternion.identity ,m_hitDistance, ~m_ignoreMask);
				Physics.Raycast(transform.position, -transform.up, out hit, m_hitDistance, ~m_ignoreMask);

				// デバッグRay描画
				Debug.DrawRay(transform.position, -transform.up, Color.red, m_hitDistance);

			});

		this.UpdateAsObservable()
            .Subscribe(_ =>
			{
				var foo = transform.localScale;
				transform.localScale = Vector3.one;
				transform.localScale = foo;

				var bar = transform.localRotation;
				transform.localRotation = Quaternion.identity;
				transform.localRotation = bar;

				Vector3 lossScale = transform.lossyScale;
				Vector3 localScale = transform.localScale;
				transform.localScale = new Vector3(
						localScale.x / lossScale.x * m_defaultScale.x,
						localScale.y / lossScale.y * m_defaultScale.y,
						localScale.z / lossScale.z * m_defaultScale.z
				);

			});


		this.ObserveEveryValueChanged(x => hit)
			.Where(x => x.transform != null)
			//.DistinctUntilChanged()
			.Subscribe(x =>
			{
				// 親を解除して衝突相手の子に設定する
				//transform.parent = null;
				transform.parent = x.transform;
				// transform.localScale = Vector3.one;
			});

		this.ObserveEveryValueChanged(x => hit)
			// .Where(_ => cg.IsGround.Value != true)
			.Where(x => x.transform == null)
			.ThrottleFrame(5)
            .TakeUntilDestroy(this)
			.Subscribe(_ =>
			{
				transform.parent = null;
				// transform.localScale = m_defaultScale;
			});
	}
}

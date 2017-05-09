// ---------------------------------------
// Brief : 接地判定
// 
// Date  : 2017/05/09
// 
// Author: Y.Watanabe
// ---------------------------------------

using UniRx;
using UniRx.Triggers;
using UnityEngine;
using System.Collections;

public class CheckGround : MonoBehaviour
{
	// 衝突判定を取る距離
	//[SerializeField]
	//private float m_hitDistance;

	private ReactiveProperty<bool> isGroundRP = new ReactiveProperty<bool>(false);

	/// <summary>
	/// 接地しているかどうか
	/// </summary>
	public IReactiveProperty<bool> IsGround
	{
		get { return isGroundRP; }
	}

	//private RaycastHit hit = new RaycastHit();

	//private bool isGround;

	void Start ()
	{
		this.OnTriggerEnterAsObservable()
			// .Where(collider => collider.transform.root.GetInstanceID() != transform.GetInstanceID())
			.Subscribe(_ => isGroundRP.Value = true);

		this.OnTriggerExitAsObservable()
			.Subscribe(_ => isGroundRP.Value = false);
		/*/
		// 衝突相手の情報を更新する
		this.UpdateAsObservable()
			.Subscribe(_ =>
			{
				isGround = Physics.Raycast(transform.position, transform.up, out hit, m_hitDistance);
				//if (hit.transform.GetInstanceID() == transform.GetInstanceID())
				//	hit = new RaycastHit();
			});

		// 衝突情報と通知
		transform.ObserveEveryValueChanged(x => x.GetComponent<CheckGround>().hit)
			.DistinctUntilChanged()
			.Subscribe(x =>
			{
				if (hit.transform != null)
					Debug.Log("hit transform" + hit.transform.name);

				isGroundRP.SetValueAndForceNotify(hit.transform != null);
				Debug.Log("notify ground");
			});

		// 衝突情報と通知を飛ばす
		this.UpdateAsObservable()
			.Subscribe(_ => Debug.Log(isGroundRP.Value));

		/*/
	}
}

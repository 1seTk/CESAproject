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
using System.Linq;

public class PlayerCollision : MonoBehaviour
{
	private Collider[] m_colliders;

	// 衝突相手
	private int[] m_hitObjects = new int[3] { -1, -1, -1 };

	// 衝突距離
	[SerializeField, Range(0, 10)]
	private float m_hitDistance;

	private ReactiveProperty<bool> isHitRP = new ReactiveProperty<bool>();

	// 衝突しているか
	public IReactiveProperty<bool> IsHit
	{
		get { return isHitRP; }
	}

	// 衝突相手のオブジェクト
	private Transform m_hitObject;

	/// <summary> 
	/// 更新前処理
	/// </summary>
	void Start ()
	{
		var rb = GetComponentInParent<Rigidbody>();
		var core = GetComponent<PlayerCore>();
		var pm = GetComponent<PlayerMover>();

		RaycastHit hit = new RaycastHit();

		// 上下左右のColliderを参照
		m_colliders = transform.GetChild(0).GetComponentsInChildren<Collider>();

		// RigitBodyのスリープ解除
		this.UpdateAsObservable()
			.Subscribe(_ => rb.WakeUp());

		// はさまれた判定を取る(左右、上下の順)
		foreach (var item in m_colliders.Select((v, i) => new { Value = v, Index = i }))
		{
			item.Value.OnTriggerStayAsObservable()
				.Subscribe(col =>
				{
					int ind = item.Index / 2;

					Debug.Log("衝突方向：" + ind + "インデックス:" + item.Index);

					int id = col.gameObject.GetInstanceID();

					if (m_hitObjects[ind] == -1)
					{
						// 代入
						m_hitObjects[ind] = id;
						Debug.Log(ind + "に" + id + "を代入" + "衝突方向：" + item.Index);
					}
					else if (m_hitObjects[ind] != id)
					{
						// はさまれた
						Debug.Log("は？" + "ind : " + (ind == 0 ? "左右" : "上下") + "id : " + id);
						core.IsDead.Value = true;
					}
				});

			// ぶつかっているものの登録を削除する
			item.Value.OnTriggerExitAsObservable()
				// 5フレーム遅延させる(一応)
				.DelayFrame(5)
				.Subscribe(_ =>
				{
					m_hitObjects[item.Index / 2] = -1;
				});
		}

		// 衝突情報と通知を飛ばす
		this.UpdateAsObservable()
			.Select(_ => Physics.Raycast(transform.position, transform.forward, out hit, m_hitDistance))
			.Subscribe(x => {
				isHitRP.SetValueAndForceNotify(x);
				m_hitObject = hit.transform;
			});

		//IsHit.Subscribe(x => core.PlayerControllable.SetValueAndForceNotify(x));
	}
}

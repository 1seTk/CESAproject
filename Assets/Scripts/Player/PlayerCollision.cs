// ---------------------------------------
// Brief : プレイヤーの当たり判定
// 
// Date  : 2016/04/24
// 
// Author: Y.Watanabe
// ---------------------------------------

using UniRx;
using UniRx.Triggers;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerCollision : MonoBehaviour
{
	[SerializeField, Range(0, 10), Tooltip("はさまれた判定を出す最短の距離")]
	private float m_crossDistance;

	// オブジェクトと接触点
	// private Dictionary<GameObject, Vector3> m_contacts = new Dictionary<GameObject, Vector3>();

	private List<GameObject> m_contactsObjects = new List<GameObject>();

	private List<Vector3> m_contactPoints = new List<Vector3>();

	/// <summary> 
	/// 更新前処理
	/// </summary>
	private void Start ()
	{
		var core = GetComponent<PlayerCore>();

		this.OnCollisionStayAsObservable()
			.Subscribe(x =>
			{
				foreach (var contact in x.contacts)
				{
					// 自分自身の判定のみを取る
					if(contact.thisCollider == GetComponent<Collider>())
					{
						var hit = contact.otherCollider.gameObject;
						// 既に接触しているか？
						if (m_contactsObjects.Contains(hit) == false)
						//if(m_contacts.ContainsKey(contact.otherCollider.gameObject) == false)
						{
							Debug.Log(contact.thisCollider.name + " hit " + contact.otherCollider.name);

							// 未登録のオブジェクトは登録する
							m_contactsObjects.Add(hit);
							m_contactPoints.Add(contact.point);
						}
						else
						{
							m_contactPoints[m_contactsObjects.IndexOf(hit)] = contact.point;
						}
					}
				}
			});

		this.OnCollisionExitAsObservable()
			.Subscribe(x =>
			{
				List<GameObject> keyList = new List<GameObject>(m_contactsObjects);
				foreach (var key in keyList)
				{
					// 判定用フラグ
					bool flg = false;

					// オブジェクトがまだ接しているかを判定する
					foreach (var contact in x.contacts)
					{
						if (contact.thisCollider.gameObject == key)
							flg = true;
						else
							flg = false;
					}

					// 接していない場合は削除する
					if (flg == false)
					{
						Debug.Log("delete " + key.name);
						int index = m_contactsObjects.IndexOf(key);
						m_contactsObjects.RemoveAt(index);
						m_contactPoints.RemoveAt(index);
					}
						
				}
			});

		this.UpdateAsObservable()
			// 2つ以上のオブジェクトに接している
			.Where(_ => m_contactPoints.Count > 1)
			.Subscribe(_ =>
			{
				for (int i = 0; i < m_contactPoints.Count - 1; i++)
				{
					float distance = Vector3.Distance(m_contactPoints[i], m_contactPoints[i + 1]);

					// 2点間の距離が規定の距離より大きい場合は挟まれた判定を出す
					if (distance <= 1.1f && distance > 0.9f/*/|| distance > m_crossDistance/*/)
					{
						Debug.Log("distance " + distance);
						core.IsDead.Value = true;
					}
				}
			});
	}

	/*/
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

					int id = col.gameObject.GetInstanceID();

					if (m_hitObjects[ind] == -1)
					{
						// 代入
						m_hitObjects[ind] = id;
					}
					else if (m_hitObjects[ind] != id)
					{
						// はさまれた
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
	/*/
}

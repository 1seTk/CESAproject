// ---------------------------------------
// Brief : プレイヤーの当たり判定
// 
// Date  : 2017/06/12
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
	private float m_minCrossDistance;

	[SerializeField, Range(0, 10), Tooltip("はさまれた判定を出す最大の距離")]
	private float m_maxCrossDistance;

	// オブジェクトと接触点
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
					if (contact.thisCollider == GetComponent<Collider>())
					{
						var hit = contact.otherCollider.gameObject;
						// 既に接触しているか？
						if (m_contactsObjects.Contains(hit) == false)
						{
							// 未登録のオブジェクトは登録する
							m_contactsObjects.Add(hit);
							m_contactPoints.Add(contact.point);
						}
						// 既に接触している場合は座標の更新のみを行う
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
					if (distance <= m_maxCrossDistance && distance > m_minCrossDistance)
					{
						core.IsDead.Value = true;
					}
				}
			});
	}
}

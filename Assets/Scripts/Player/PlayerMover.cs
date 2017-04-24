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
		this.UpdateAsObservable()
			.Where(_ => Input.GetKey(KeyCode.Space))
			.Where(_ => m_canMove == true)
			.Subscribe(_ =>
			{
				Debug.Log("Pressed Space");
				transform.position += new Vector3(0, 0, m_speed) * Time.deltaTime;
			});

		// 前方にオブジェクトが存在するか
		this.UpdateAsObservable()
			// .Where(_ => Physics.Raycast(transform.position, transform.forward, 10))
			.Subscribe(_ =>
			{
				Ray ray = new Ray(transform.position, transform.forward);

				// Rayの可視化
				Debug.DrawRay(ray.origin, ray.direction, Color.red, 1.0f);

				if (Physics.Raycast(ray, 0.5f))
				{
					Debug.Log("Front Ray Hit");
					m_canMove = false;
				}
				else
				{
					m_canMove = true;
				}
			});
	}
}

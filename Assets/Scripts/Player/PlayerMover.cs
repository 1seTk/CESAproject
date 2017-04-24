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
	}
}

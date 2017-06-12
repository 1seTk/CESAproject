// ---------------------------------------
// Brief : プレイヤーのデバッグ機能
// 
// Date  : 2017/05/15
// 
// Author: Y.Watanabe
// ---------------------------------------

using UniRx;
using UniRx.Triggers;
using UnityEngine;
using System.Collections;

public class PlayerDebug : MonoBehaviour
{
	[SerializeField, Range(0, 10)]
	private float m_backSpeed = 5.0f;

	void Start ()
	{
		var core = GetComponent<PlayerCore>();

		this.UpdateAsObservable()
			.Where(_ => Input.GetKey(KeyCode.LeftShift))
			.Where(_ => Input.GetKey(KeyCode.D))
			.Subscribe(_ =>
			{
				core.IsDead.Value = true;
			});

		// 逆走
		this.UpdateAsObservable()
			.Where(_ => Input.GetMouseButton(1))
			.Where(_ => core.PlayerControllable.Value == true)
			.Subscribe(_ => transform.position -= new Vector3(0, 0, m_backSpeed) * Time.deltaTime);

	}
}

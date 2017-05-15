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
	}
}

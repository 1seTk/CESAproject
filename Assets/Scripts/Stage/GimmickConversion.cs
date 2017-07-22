// ---------------------------------------
// Brief : ギミックの変換用スクリプト
// 
// Date  : 2017/05/05
// 
// Author: Y.Watanabe
// ---------------------------------------

using UniRx;
using UnityEngine;
using System.Collections;

public class GimmickConversion : MonoBehaviour
{
	/// <summary> 
	/// 更新前処理
	/// </summary>
	void Start ()
	{
		var cores = GetComponentsInChildren<GimmickCore>();

		foreach (var item in cores)
		{
			Debug.Log("freeze core");
			item.enabled = false;
		}

		var targets = GetComponentsInChildren<GimmickTarget>();

		Debug.Log("target cap");

		for (int i = 0; i < targets.Length; i++)
		{
			Debug.Log("replacing...");

			targets[i].gameObject.AddComponent<GimmickMover>().Converse(targets[i]);

			// ギミック自身へのTweenの場合の例外
			if (targets[i].GetComponent<GimmickCore>() != null)
			{
				targets[i].GetComponent<GimmickMover>().m_isReset = true;
			}

			Debug.Log("Destroy");

			Destroy(targets[i]);
		}

		foreach (var item in cores)
		{
			Debug.Log("reboot core");
			item.enabled = true;
		}

		Debug.Log("replace successed !");
	}
}

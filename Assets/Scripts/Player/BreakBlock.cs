using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakBlock : MonoBehaviour {

	[SerializeField, Tooltip("死んだときに破裂するか")]
	private bool m_useBomb = true;

	[SerializeField, Tooltip("パーツのプレハブ")]
	private GameObject m_parts;

	public void Break()
	{
		// 破片の生成
		var obj = Instantiate(m_parts, transform.position, Quaternion.identity) as GameObject;

		// 破片の参照を取る
		var parts = obj.GetComponentsInChildren<Transform>();

		// 破片にいろいろ施す
		foreach (var item in parts)
		{
			if(m_useBomb)
				item.gameObject.layer = LayerMask.NameToLayer("Default");

			if(!item.GetComponent<Rigidbody>())
				item.gameObject.AddComponent<Rigidbody>();
			item.parent = null;
		}

		Destroy(gameObject, 0.1f);
		Destroy(transform.gameObject, 0.1f);
	}
}

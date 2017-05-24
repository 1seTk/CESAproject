// ---------------------------------------
// Brief : 
// 
// Date  : 
// 
// Author: Y.Watanabe
// ---------------------------------------

using UniRx;
using UniRx.Triggers;
using UnityEngine;
using System.Collections;

public class PlayerFriction : MonoBehaviour
{
	// 衝突距離
	[SerializeField, Range(0, 10)]
	private float m_hitDistance;


	void Start ()
	{
		// 四方にレイを飛ばす

		// レイにヒットしたオブジェクトを方向のリストに
		// 方向のリストないでの合計移動量を計算
		// 計算結果をプレイヤーに反映

		// or

		// ギミックに速度を持たせる
		// レイにヒットしたオブジェクトの速度を合計
		// 計算結果をプレイヤーに反映

		// レイにヒットしたオブジェクトに追従する
		// ただし、前面のレイはXY方向
		// 左右面はYZ方面のみに追従する
		// Y方向のマイナスは0に戻す
		// XZ方向は少しづつ減らす

		//// 衝突情報と通知を飛ばす
		//this.UpdateAsObservable()
		//	.Select(_ => Physics.Raycast(transform.position, transform.forward, out hit, m_hitDistance))
		//	.Subscribe(x => {
		//		isHitRP.SetValueAndForceNotify(x);
		//		m_hitObject = hit.transform;
		//		Debug.Log(m_hitObject.name);
		//	});

		RaycastHit p = Physics.Raycast(transform.position, transform.forward, out hit, m_hitDistance);
		// 衝突情報と通知を飛ばす
		this.UpdateAsObservable()
			.Select(_ => p)
			.Subscribe(x =>
			{
				isHitRP.SetValueAndForceNotify(x);
				m_hitObject = hit.transform;
				Debug.Log(m_hitObject.name);
			});

	}
}

public class RayTest : MonoBehaviour
{

	public LayerMask mask;

	void Ray ()
	{

		// 1.
		// Rayの作成
		Ray ray = new Ray(transform.position, transform.forward);
		Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

		// 2.		
		// Rayが衝突したコライダーの情報を得る
		RaycastHit hit;
		// Rayが衝突したかどうか
		if (Physics.Raycast(ray, out hit, 10.0f, mask))
		{
			// Examples
			// 衝突したオブジェクトの色を赤に変える
			hit.collider.GetComponent<MeshRenderer>().material.color = Color.red;
			// 衝突したオブジェクトを消す
			Destroy(hit.collider.gameObject);
			// Rayの衝突地点に、このスクリプトがアタッチされているオブジェクトを移動させる
			this.transform.position = hit.point;
			// Rayの原点から衝突地点までの距離を得る
			float dis = hit.distance;
			// 衝突したオブジェクトのコライダーを非アクティブにする
			hit.collider.enabled = false;
		}

		// 3.		
		// Rayが衝突した全てのコライダーの情報を得る。＊順序は保証されない
		RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity);
		foreach (var obj in hits)
		{
			// 衝突したオブジェクトのタグ毎に色を変え、当てはまらない場合はオブジェクトを非アクティブにする
			switch (obj.collider.tag)
			{
				case "Red":
					obj.collider.GetComponent<MeshRenderer>().material.color = Color.red;
					break;
				case "Blue":
					obj.collider.GetComponent<MeshRenderer>().material.color = Color.blue;
					break;
				case "Green":
					obj.collider.GetComponent<MeshRenderer>().material.color = Color.green;
					break;
				default:
					obj.collider.gameObject.SetActive(false);
					break;
			}
		}

		// 4.		
		// 球形のRayをとばす
		if (Physics.SphereCast(ray, 5.0f, out hit, 10.0f)) { }

		// 5.		
		// Rayの可視化
		Debug.DrawRay(ray.origin, ray.direction, Color.red, 3.0f);

		// 6.		
		// マウスがEventSystem上にあるか
		UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
		// マウスがEventSystem上にないときに、左クリックした場合
		if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() && Input.GetMouseButtonDown(0))
		{
			// 処理内容
		}
	}
}
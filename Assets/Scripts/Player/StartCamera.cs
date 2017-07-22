// ---------------------------------------
// Brief : 開始演出
// 
// Date  : 2016/06/22
// 
// Author: Y.Watanabe
// ---------------------------------------

using UnityEngine;
using System.Collections;
using DG.Tweening;
using UniRx;
using UniRx.Triggers;

public class StartCamera : MonoBehaviour
{
	private GoalDetector m_goal;

	void Start ()
	{
		// リロード時は処理をしない
		if (YamagenLib.PlayInstructor.instance.IsReload()) Destroy(gameObject);

		// 演出中断のための入力
		var input = GetComponentInParent<IPlayerInput>();

		// ゴールへの参照
		m_goal = GameObject.Find("GoalEx").GetComponent<GoalDetector>();

		// ワールド座標で移動するので親をヌルヌル
		transform.parent = null;

		StartCoroutine(MoveCamera());

		// ジャンプボタンが押されたら演出中断
		input.OnJumpButtonObservable
			.Where(x => x == true)
            .TakeUntilDestroy(this)
			.Subscribe(_ =>
			{
				StopCoroutine(MoveCamera());
				StopCamera();
			});
	}

	IEnumerator MoveCamera()
	{
		// スタートからゴールまで移動する
		transform.DOMoveZ(m_goal.transform.position.z, 10.0f);

		yield return new WaitForSeconds(5);

		ShunLib.FadeScene.instance.FadeIn();

		// フェードアウト待機
		yield return new WaitForSeconds(2);

		// 役目が終わったら消す
		Destroy(gameObject);
	}

	void StopCamera()
	{
		ShunLib.FadeScene.instance.FadeIn();

		Destroy(gameObject, 2.0f);
	}

	private void OnDestroy ()
	{
		ShunLib.FadeScene.instance.FadeOut();
	}
}

// ---------------------------------------
// Brief : ゴール判定用
// 
// Date  : 2017/04/27
// 
// Author: Y.Watanabe
// ---------------------------------------

using UniRx;
using UniRx.Triggers;
using UnityEngine;
using System.Collections;

public class GoalDetector : MonoBehaviour
{
	/// <summary> 
	/// 更新前処理
	/// </summary>
	void Start ()
	{
		var ins = FindObjectOfType<YamagenLib.SceneInstructor>();

		this.OnTriggerEnterAsObservable()
			.DistinctUntilChanged()
			.Subscribe(_ =>
			{
				Debug.Log("Goal");
				ins.LoadMainScene(YamagenLib.GameScene.Clear);
			});
	}
}

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
    private bool m_isGoal = false;

	/// <summary>
	/// 更新前処理
	/// </summary>
	void Start ()
	{
		var ins = FindObjectOfType<YamagenLib.SceneInstructor>();

		this.OnTriggerEnterAsObservable()
            .Where(x => x.name == "Player")
			.DistinctUntilChanged()
			.Subscribe(_ =>
			{
				Debug.Log("Goal");
                m_isGoal = true;
			});
	}

    public bool GetState()
    {
        return m_isGoal;
    }

    public void SetDefault()
    {
        m_isGoal = false;
    }
}

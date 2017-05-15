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
using DG.Tweening;

public class GoalDetector : MonoBehaviour
{
    private GameObject _player;

    [SerializeField]
    private float _duration = 2.0f;

    YamagenLib.SceneInstructor ins;

	/// <summary> 
	/// 更新前処理
	/// </summary>
	void Start ()
	{
        _player = GameObject.Find("Player");

		ins = FindObjectOfType<YamagenLib.SceneInstructor>();

		this.OnTriggerEnterAsObservable()
			.DistinctUntilChanged()
			.Subscribe(_ =>
			{
				Debug.Log("Goal");

                _player.transform.DOScaleY(0, _duration);

            });
	}

    void Update()
    {
        if (_player.transform.localScale.y <= 1)
        {
            Debug.Log("scene change");

            ins.LoadMainScene(YamagenLib.GameScene.Clear);
        }
    }

}

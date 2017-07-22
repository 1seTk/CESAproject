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
    void Start()
    {

        var ins = FindObjectOfType<YamagenLib.SceneInstructor>();

        // ゴール用サウンドのセット
        AudioManager.Instance.AudioSet("Goal", "MP3\\Goal");

        this.OnTriggerEnterAsObservable()
            .Where(x => x.name == "Player")
            .DistinctUntilChanged()
            .Subscribe(player =>
            {
                // ゴール用サウンドを再生
                AudioManager.Instance.Play("Goal");

                Debug.Log("Goal");
                m_isGoal = true;

                StartCoroutine(PlayerGoal(player.GetComponent<PlayerCore>()));
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

    IEnumerator PlayerGoal(PlayerCore core)
    {
        yield return new WaitForSeconds(0.5f);

        core.Invincible = true;
    }
}

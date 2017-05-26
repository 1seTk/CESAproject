using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    static public StageManager instance;

    [SerializeField]
    private bool m_isClear;

    [SerializeField]
    private bool m_isOver;

    [SerializeField]
    private PlayerCore m_core;

    [SerializeField]
    private GoalDetector m_goal;

    /// <summary>
    /// 初期化
    /// </summary>
    private void Awake()
    {
        // シングルトン
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Update()
    {
        if(m_goal.GetState()||m_isClear)
        {
            // ゴールした時
            ShunLib.ClearDirection.instance.GameClear();
        }

        if (m_core.IsDead.Value||m_isOver)
        {
            // 死んだ時
            ShunLib.GameOverDirection.instance.GameOver();
        }
    }

}

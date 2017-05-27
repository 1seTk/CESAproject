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
        if(m_goal.GetState())
        {
            m_isClear = true;
        }

        if (m_core.IsDead.Value)
        {
            m_isOver = true;
        }

        GameClear();
        GameOver();
    }

    private void GameClear()
    {
        if (m_isClear)
        {
            // ゴールした時
            ShunLib.ClearDirection.instance.GameClear();
            m_isClear = false;
        }
    }

    private void GameOver()
    {
        if (m_isOver && m_isClear == false) 
        {
            // 死んでクリアしていない時
            ShunLib.GameOverDirection.instance.GameOver();
            m_isOver = false;
        }
    }

}

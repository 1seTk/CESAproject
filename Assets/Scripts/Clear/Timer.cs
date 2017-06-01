using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{

    [SerializeField, Tooltip("Player")]
    private GameObject  m_player;

    private PlayerCore  m_playerCore;
    private PlayerEnter m_playerEnter;

    // 経過時間
    private float m_elapsedTime = 0.0f;

    void Start ()
    {
        m_playerCore = m_player.GetComponent<PlayerCore>();
        m_playerEnter = m_player.GetComponent<PlayerEnter>();
    }


    // 更新処理
    void Update ()
    {
        //時間描画
        DrawTime();

        //プレイヤーが登場するまでカウントしない
        if (!(m_playerEnter.IsPlayerEnter)) return;

        //プレイヤーが生きている間カウントする
        if (!(m_playerCore.IsDead.Value))
        {
            m_elapsedTime += Time.deltaTime;
        }

	}

    IEnumerator WaitStartPlayer()
    {
        // プレイヤーが登場し終えるまで待機
        while (m_playerEnter.IsPlayerEnter == false)
        {
            yield return new WaitForEndOfFrame();
        }
    }

    /// <summary>
    /// 経過時間をリセット
    /// </summary>
    void Reset()
    {
        m_elapsedTime = 0.0f;
    }


    /// <summary>
    /// 経過時間を取得
    /// </summary>
    /// <returns>経過時間</returns>
    float GetTime()
    {
        return m_elapsedTime;
    }

    /// <summary>
    /// タイムを表示
    /// </summary>
    void DrawTime()
    {
        float time = m_elapsedTime;

        //分・秒・ミリ秒
        int minute = (int)time / 60;
        int second = (int)time % 60;
        int millisecond = (int)((time - (int)time) * 100) ;

        GetComponent<Text>().text = minute.ToString().PadLeft(2,'0') +":"+ second.ToString().PadLeft(2, '0') + ":"+ millisecond.ToString().PadLeft(2, '0');
    }
}

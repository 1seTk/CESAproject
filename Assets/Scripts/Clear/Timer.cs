using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    //[SerializeField, Tooltip("Player")]
    private PlayerCore m_playerCore;

    //プレイヤー情報取得用
    private PlayerEnter m_playerEnter;

    //時間描画用
    private Text m_text;

    // 経過時間
    static private float m_elapsedTime = 0.0f;

    //クリア処理をしたかどうか
    private bool m_isClear = false;

    //ベストタイムを記録するキー
    private string m_recordKey;

    void Start()
    {
        m_playerCore = GameObject.Find("Player").GetComponentInChildren<PlayerCore>();
        m_playerEnter = m_playerCore.transform.GetComponent<PlayerEnter>();
        m_text = GetComponent<Text>();
        m_recordKey = YamagenLib.PlayInstructor.instance.GetLoadStage().ToString() + "Time";
        m_elapsedTime = 0.0f;
    }


    // 更新処理
    void Update()
    {
        //時間描画
        if (!m_isClear) m_text.text = ConvertStringTime(m_elapsedTime);
        else m_text.text = "";

        //プレイヤーが登場するまでカウントしない
        if (!(m_playerEnter.IsPlayerEnter)) return;

        //プレイヤーが生きていてゴールしていない間カウントする
        if ((StageManager.instance.TimerStop() == false))
        {
            m_elapsedTime += Time.deltaTime;
        }
        else
        {
            //クリア後の処理をする
            if (!m_isClear)
            {
                RecordTime();
                m_isClear = true;
            }
        }

        // 死んでたら消す
        if (m_playerCore == null ) m_text.text = "";
    }

    /// <summary>
    /// 経過時間をリセット
    /// </summary>
    public void Reset()
    {
        m_elapsedTime = 0.0f;
    }


    /// <summary>
    /// 経過時間を取得
    /// </summary>
    /// <returns>経過時間</returns>
    static public float GetTime()
    {
        return m_elapsedTime;
    }

    /// <summary>
    /// タイムを文字列に変換して返す
    /// </summary>
    static public string ConvertStringTime(float time)
    {
        //分・秒・ミリ秒
        int minute = (int)time / 60;
        int second = (int)time % 60;
        int millisecond = (int)((time - (int)time) * 100);

        return (minute.ToString().PadLeft(2, '0') + ":" + second.ToString().PadLeft(2, '0') + ":" + millisecond.ToString().PadLeft(2, '0'));
    }

    /// <summary>
    /// ベストタイムを更新していたら記録
    /// </summary>
    void RecordTime()
    {
        float bestTime = PlayerPrefs.GetFloat(m_recordKey, float.MaxValue);

        //ベストタイムより速かったら記録
        if (bestTime > m_elapsedTime)
        {
            Debug.Log("Record");
            PlayerPrefs.SetFloat(m_recordKey, m_elapsedTime);
        }
    }
}

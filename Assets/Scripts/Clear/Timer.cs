using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    [SerializeField, Tooltip("Player")]
    private GameObject m_player;

    //プレイヤー情報取得用
    private PlayerCore m_playerCore;
    private PlayerEnter m_playerEnter;

    //時間描画用
    private Text m_text;

    // 経過時間
    private float m_elapsedTime = 0.0f;

    //クリア処理をしたかどうか
    private bool m_isClear = false;

    //ベストタイムを記録するキー
    private string m_recordKey;

    void Start ()
    {
        m_playerCore = m_player.GetComponent<PlayerCore>();
        m_playerEnter = m_player.GetComponent<PlayerEnter>();
        m_text = GetComponent<Text>();
        m_recordKey = YamagenLib.SelectManager.instance.GetSelectStage().ToString() + "Time";

        Debug.Log(YamagenLib.SelectManager.instance.GetSelectStage().ToString());
    }


    // 更新処理
    void Update ()
    {
        //時間描画
        //m_text.text = ConvertStringTime(m_elapsedTime);
        m_text.text = ConvertStringTime(PlayerPrefs.GetFloat(m_recordKey, float.MaxValue));

        //プレイヤーが登場するまでカウントしない
        if (!(m_playerEnter.IsPlayerEnter)) return;

        //プレイヤーが生きていてゴールしていない間カウントする
        if (!(m_playerCore.IsDead.Value))
        {
            if ((ShunLib.ClearDirection.instance.isClear() == false))
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
        }
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
    public float GetTime()
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
        int millisecond = (int)((time - (int)time) * 100) ;

        return (minute.ToString().PadLeft(2,'0') +":"+ second.ToString().PadLeft(2, '0') + ":"+ millisecond.ToString().PadLeft(2, '0'));
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UniRx;
using UniRx.Triggers;


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

    // スローモーション値の許容値(フレーム)
    [SerializeField]
    private int m_acceptSlowTime = 200;

    // スローモーション時間カウント用
    [SerializeField]
    private int m_slowCnt;

    // メインカメラ
    private GameObject m_camera;

    // タイマー停止用
    private bool m_stopFlag = false;

    public bool IsClear
    {
        get { return m_isClear; }
    }

    /// <summary>
    /// 初期化
    /// </summary>
    private void Awake()
    {
        // シングルトン
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }
    private void Start()
    {
        AudioManager.Instance.AudioSet("Dead", "MP3\\GrassBroken");
        AudioManager.Instance.SetVolume("Dead", 0.6f);

        // メインカメラみーつけた
        m_camera = GameObject.Find("Main Camera");

    }

    private void Update()
    {
        if(m_goal.GetState())
        {
            m_isClear = true;
            GoalEffect();
            m_goal.SetDefault();
        }

        if (m_core.IsDead.Value && (ShunLib.ClearDirection.instance.isClear()==false))
        {
            m_isOver = true;
            m_core.Resurrection();
            AudioManager.Instance.Play("Dead");
        }

        GameClear();
        GameOver();
    }

    private void GameClear()
    {
        // ゴールに触れたらスローモーションに
        if (m_isClear)
        {
            m_stopFlag = true;
            m_slowCnt++;

            Time.timeScale = 0.5f;
        }

        // スローモーション経過時間が許容値を超えた場合
        if (m_slowCnt > m_acceptSlowTime)
        {
            m_slowCnt = 0;
            Time.timeScale = 1.0f;
            ShunLib.ClearDirection.instance.GameClear();

            m_isClear = false;
        }
    }

    private void GameOver()
    {
        if (m_isOver && (ShunLib.ClearDirection.instance.isClear() == false)) 
        {
            // 死んでクリアしていない時
            ShunLib.GameOverDirection.instance.GameOver();
            m_isOver = false;
        }
    }

    /// <summary>
    /// ゴールした時の演出
    /// </summary>
    private void GoalEffect()
    {
        Vector3[] path = new Vector3[2];
        if (m_camera.transform.position.x > 1f)
        {
            // 時計回り
            path[0] =

            new Vector3(m_camera.transform.position.x - 4f, m_camera.transform.position.y - 2f, m_camera.transform.position.z + 7f);
        }
        else
        {
            // 反時計回り
            // 座標を配列にぶち込む
            path[0] =

            new Vector3(m_camera.transform.position.x + 4f, m_camera.transform.position.y - 2f, m_camera.transform.position.z + 7f);
        }

        if (m_camera.transform.position.x > 1f)
        {
            // 時計
            // 上で入れるとバグったので2番目の座標はこっちで代入
            path[1] = new Vector3(path[0].x + 4f, path[0].y - 3f, path[0].z + 5f);
        }
        else
        {
            // 半時計
            // 上で入れるとバグったので2番目の座標はこっちで代入
            path[1] = new Vector3(path[0].x - 4f, path[0].y - 3f, path[0].z + 5f);

        }
        // ゴール時エフェクト
        m_camera.transform.parent = null;

        // 与えた座標をに従ってカーブモーション
        if (m_camera.transform.position.x > 1f)
        {
            m_camera.transform.DOLocalPath(path, 2f, PathType.CatmullRom);

            // 回転
            m_camera.transform.DORotate(new Vector3(0, 180f, 0), 2f);

        }
        else
        {
            m_camera.transform.DOLocalPath(path, 2f, PathType.CatmullRom);

            // 回転
            m_camera.transform.DORotate(new Vector3(0, -180f, 0), 2f);

        }
    }

    public bool TimerStop()
    {
        return m_stopFlag;
    } 
}

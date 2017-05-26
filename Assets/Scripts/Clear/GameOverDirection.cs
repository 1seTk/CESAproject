using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace ShunLib
{
    public class GameOverDirection : MonoBehaviour
    {
        // シングルトン
        static public GameOverDirection instance;

        // 終点
        [SerializeField, Tooltip("移動する距離")]
        private float m_targetPositionX = 0.0f;

        //動かすオブジェクト
        [SerializeField, Tooltip("移動させるオブジェクト")]
        private GameObject[] m_obj;

        //時間
        [SerializeField, Tooltip("移動にかかる時間")]
        private float[] m_time;

        //待ち時間
        [SerializeField, Tooltip("移動が始まるまでの時間")]
        private float[] m_intervalTime;

        //画面を暗くする
        [SerializeField, Tooltip("画面を暗くする")]
        private Image m_black;

        //開始した時間
        private float m_startTime = 0.0f;

        //クリア判定とスタート判定
        private bool m_isGameOver = false;
        private bool m_isStarted = false;

        /// <summary>
        /// 初期化
        /// </summary>
        private void Awake()
        {
            // シングルトン
            if (instance == null) instance = this;
            else Destroy(gameObject);
        }

        /// <summary>
        /// 初期設定
        /// </summary>
        void Start()
        {
            m_startTime = Time.time;

            //念のため透明にする
            var color = m_black.color;
            color.a = 0.0f;
            m_black.color = color;
        }


        /// <summary>
        /// 更新処理
        /// </summary>
        void Update()
        {
            // 死んでいなかったら更新しない
            if (!IsStarted())
            {
                return;
            }

            //画面を暗くする
            DarkenScreen();

            //オブジェクトを移動させる
            MoveObject();
        }


        /// <summary>
        /// クリア演出を始めるかどうか
        /// </summary>
        private bool IsStarted()
        {
            // 死んでいなければ更新しない
            if (!(m_isGameOver))
            {
                return m_isStarted;
            }
            else
            {
                if (!m_isStarted)
                {
                    m_startTime = Time.time;
                    m_isStarted = true;
                }
            }
            return m_isStarted;
        }


        /// <summary>
        /// オブジェクトを移動させる
        /// </summary>
        private void MoveObject()
        {
            float timeStep = (Time.time - m_startTime);

            //オブジェクトが存在するならば移動させる
            if (m_obj.Length > 0)
            {
                for (int i = 0; i < m_obj.Length; i++)
                {
                    if (m_intervalTime[i] < timeStep)
                    {
                        m_obj[i].transform.DOLocalMoveX(m_targetPositionX, m_time[i]).SetEase(Ease.Linear);
                    }
                }
            }
        }


        /// <summary>
        /// 画面を暗くする
        /// </summary>
        private void DarkenScreen()
        {
            float timeStep = (Time.time - m_startTime);

            float alpha = timeStep / m_intervalTime[0];
            var color = m_black.color;

            if (alpha > 0.5f)
            {
                alpha = 0.5f;
            }
            color.a = alpha;
            m_black.color = color;
            Debug.Log(alpha);

        }
        /// <summary>
        /// 死んだら呼ぶ
        /// </summary>
        public void GameOver()
        {
            Debug.Log("GameOver");
            m_isGameOver = true;
        }
    }
}

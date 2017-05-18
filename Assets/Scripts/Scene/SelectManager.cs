using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;  // どついーん使用

namespace YamagenLib
{
    public class SelectManager: MonoBehaviour
    {
        // シングルトン
        static public SelectManager instance;

        // 回転させるオブジェクト
        [SerializeField]
        private GameObject m_obj;

        // 回転時間
        [SerializeField]
        private float m_rotateTime = 1.0f;

        // タッチ用
        private float m_startPos;           // タッチ開始座標
        private float m_endPos;             // タッチ終了座標
        private float m_screenWidth;        // 画面の横幅

        // マウス用
        private float m_MstartPos;           // マウス開始座標
        private float m_MendPos;             // マウス終了座標

        // スワイプ許容割合
        [SerializeField,Range(0.0f,1.0f)]
        private float m_AcceptableValue;
        private float m_MoveTouchX;         // 横軸移動量
        private float m_MoveMouseX;         // 横軸移動量

        // 回転tween
        private Tweener m_rotation;         // 回転

        /// <summary>
        /// 初期化
        /// </summary>
        void Awake()
        {
            // シングルトン
            if (instance == null){
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else{
                Destroy(gameObject);
            }

            // 画面の横幅
            m_screenWidth = Screen.width;

            // ついーん初期化
            DOTween.Init();
        }

        /// <summary>
        /// 更新
        /// </summary>
        void Update()
        {
            TouchUpdate();
        }

        /// <summary>
        /// タッチの場合
        /// </summary>
        void TouchUpdate()
        {
            // 一か所タッチされている時
            if (Input.touchCount == 1)
            {
                // タッチの状態検出
                Touch touch = Input.GetTouch(0);
                switch (touch.phase)
                {
                    case TouchPhase.Began:      // タッチ開始時

                        // ログ
                        Debug.Log("タッチ！");

                        // タッチ開始座標
                        m_startPos = touch.position.x;

                        break;
                    case TouchPhase.Moved:      // タッチ移動時
                    case TouchPhase.Stationary: // タッチ静止時
                        break;
                    case TouchPhase.Ended:      // タッチ終了時

                        // 横移動量画面割合(-1<tx<1)
                        m_MoveTouchX = (touch.position.x - m_startPos) / m_screenWidth; 

                        if ((m_MoveTouchX > m_AcceptableValue) || (m_MoveTouchX < -m_AcceptableValue))
                        {
                            // スワイプが許容内の場合
                            // 回転
                            ObjectRotate(m_MoveTouchX);
                        }

                        // 移動量リセット
                        m_MoveTouchX = 0.0f;                       
                        break;
                    case TouchPhase.Canceled:   // タッチキャンセル時
                        break;
                    default:                    // その他  
                        break;
                }
            }
        }

        /// <summary>
        /// マウスの場合
        /// </summary>
        void MouseUpdate()
        {
            // マウスが押された時
            if (Input.GetMouseButtonDown(0))
            {
                m_MstartPos = Input.mousePosition.x;             // プッシュ開始座標
            }
            // マウスが離された時
            if (Input.GetMouseButtonUp(0))
            {
                // 横移動量画面割合(-1<tx<1)
                m_MoveMouseX = (Input.mousePosition.x - m_MstartPos) / m_screenWidth;

                if ((m_MoveMouseX > m_AcceptableValue) || (m_MoveMouseX < -m_AcceptableValue))
                {
                    // 許容値内でスワイプの場合

                    // 回転
                    ObjectRotate(m_MoveMouseX); 
                }
                m_MoveMouseX = 0.0f;                       // 移動量リセット
            }
        }

        /// <summary>
        /// オブジェクトの回転
        /// </summary>
        /// <param name="moveDir">移動割合</param>
        public void ObjectRotate(float moveDir)
        {
            // 1か-1にする
            moveDir /= Mathf.Abs(moveDir);

            // 回転
            if (m_rotation == null)
            {
                m_rotation =
                    m_obj.transform.DORotate(
                    new Vector3(0, m_obj.transform.eulerAngles.y + (-90 * moveDir), 0),    // 終了時点のRotation
                    m_rotateTime                        // アニメーション時間
                ).OnKill(() => m_rotation = null);
            }
        }
    }
}

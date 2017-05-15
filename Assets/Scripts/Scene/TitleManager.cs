using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YamagenLib
{
    public class TitleManager: MonoBehaviour
    {
        /// <summary>
        /// 回転させるオブジェクト
        /// </summary>
        [SerializeField]
        private GameObject obj;

        //回転用
        private float m_startPos;   // タッチ開始座標
        private float m_endPos;     // タッチ終了座標
        Quaternion m_startRotate;   //タッチしたときの回転
        float m_screenWidth;          // 画面の横幅
        float tx;                   //変数

        /// <summary>
        /// 初期化
        /// </summary>
        void Awake()
        {
            // 画面の横幅
            m_screenWidth = Screen.width;
        }

        /// <summary>
        /// 更新
        /// </summary>
        void Update()
        {
            // 一か所タッチされている時
            if (Input.touchCount == 1)
            {
                // タッチの状態検出
                Touch t1 = Input.GetTouch(0);
                // タッチ開始時
                if (t1.phase == TouchPhase.Began)
                {
                    Debug.Log("タッチ！");
                    m_startPos = t1.position.x;             // タッチ開始座標
                    m_startRotate = obj.transform.rotation; // タッチ開始時の回転
                }
                // タッチ移動
                else if (t1.phase == TouchPhase.Moved|| t1.phase == TouchPhase.Stationary)
                {
                    tx = (t1.position.x - m_startPos) / m_screenWidth; //横移動量(-1<tx<1)
                    obj.transform.rotation = m_startRotate;
                    // 回転
                    obj.transform.Rotate(new Vector3(0, -90 * tx, 0), Space.World);
                }
                // タッチ終了時
                else if (t1.phase == TouchPhase.Stationary)
                {
                }
            }
        }

        void CubeRotate()
        {

        }

    }
}

//************************************************/
//* @file  :ClearDirection.cs
//* @brief :クリア画面の演出
//* @date  :2017/05/23
//* @author:S.Katou
//************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ShunLib
{
    public class ClearDirection : MonoBehaviour
    {
        //秒数
        [SerializeField, Range(0.0f, 10.0f)]
        private float m_second = 0.0f;

        // 始点
        [SerializeField]
        private Vector3 m_startPosition = Vector3.zero;
        // 終点
        [SerializeField]
        private Vector3 m_targetPosition = Vector3.zero;

        private float m_autoMoveTime = 0.0f;

        [SerializeField]
        private GameObject[] obj;

        /// <summary>
        /// 初期設定
        /// </summary>
        void Start()
        {
            m_autoMoveTime = Time.time;
        }


        /// <summary>
        /// 更新処理
        /// </summary>
        void Update()
        {
            float timeStep = (Time.time - m_autoMoveTime) / m_second;

            transform.position = Lerp(m_startPosition, m_targetPosition, timeStep, Cos);
        }


        private Vector3 Lerp(Vector3 startPosition, Vector3 targetPosition, float t, Func<float, float> v)
        {
            Vector3 lerpPosition = Vector3.zero;

            lerpPosition = (1 - v(t)) * startPosition + v(t) * targetPosition;

            return lerpPosition;
        }

        //線形補間
        private float Linearity(float time)
        {
            float vt = 0.0f;

            vt = time;

            return vt;
        }
    }
}

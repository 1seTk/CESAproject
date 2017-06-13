using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YamagenLib
{

    public class TitleManager : MonoBehaviour
    {
        // シングルトン
        static public TitleManager instance;

        // タッチマウス用
        private Vector3 m_startPos;           // タッチ開始座標
        private Vector3 m_endPos;             // タッチ終了座標
        private float m_screenWidth;        // 画面の横幅

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
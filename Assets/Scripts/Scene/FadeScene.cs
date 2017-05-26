using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShunLib
{
    public class FadeScene : MonoBehaviour
    {
        // シングルトン
        static public FadeScene instance;

        //フェード用のクラス取得用
        private Fade m_fade = null;

        //フェード用のクラス取得用
        private FadeImage m_fadeImage = null;


        //フェードイン用のテクスチャ
        [SerializeField]
        private Texture m_fadeInMaskTexture = null;

        //フェードインするかどうか
        [SerializeField]
        private bool m_fadeIn;
        public bool isFadeIn
        {
            set { this.m_fadeIn = value; }
        }


        //フェードインの時間
        [SerializeField, Range(0, 10)]
        private float m_fadeInTime;
        public float FadeInTime
        {
            set { this.m_fadeInTime = value; }
        }

        //フェードアウトするかどうか
        [SerializeField]
        private bool m_fadeOut;
        public bool isFadeOut
        {
            set { this.m_fadeOut = value; }
        }

        //フェードアウト用のテクスチャ
        [SerializeField]
        private Texture m_fadeOutMaskTexture = null;


        //フェードアウトの時間
        [SerializeField, Range(0, 10)]
        private float m_fadeOutTime;
        public float FadeOutTime
        {
            set { this.m_fadeOutTime = value; }
        }


        //フェードインするかどうか
        [SerializeField]
        private bool m_fadeInOut;
        public bool isFadeInOut
        {
            set { this.m_fadeInOut = value; }
        }

        private bool m_isCallBack;

        /// <summary>
        /// 初期化
        /// </summary>
        private void Awake()
        {
            // シングルトン
            if (instance == null) instance = this;
            else Destroy(gameObject);
        }

        // Use this for initialization
        //初期化
        void Start()
        {
            m_fade = gameObject.GetComponent<Fade>();
            m_fadeImage = gameObject.GetComponent<FadeImage>();
            m_isCallBack = false;
        }


        // Update is called once per frame
        //更新
        void Update()
        {
            if (CallBack(0))
            {
                m_isCallBack = false;
            }

            //フェードイン単体
            if (m_fadeIn)
            {
                m_fadeImage.UpdateMaskTexture(m_fadeInMaskTexture);
                m_fade.FadeIn(m_fadeInTime);
                m_fadeIn = false;
            }

            //フェードアウト単体
            if (m_fadeOut)
            {
                m_fadeImage.UpdateMaskTexture(m_fadeOutMaskTexture);
                m_fade.FadeOut(m_fadeOutTime);
                m_fadeOut = false;
            }

            //フェードイン　ー>　フェードアウト
            if (m_fadeInOut)
            {
                m_fadeImage.UpdateMaskTexture(m_fadeInMaskTexture);
                m_fade.FadeIn(m_fadeInTime, CallBack);
                m_fadeInOut = false;
            }

            if (CallBack(0))
            {
                m_fadeImage.UpdateMaskTexture(m_fadeOutMaskTexture);
                m_fade.FadeOut(m_fadeOutTime);
                //m_isCallBack = false;
            }
        }


        //フェードイン
        public void FadeIn()
        {
            m_fadeIn = true;
        }

        //フェードインアウト
        public void FadeOut()
        {
            m_fadeOut = true;
        }

        //フェードイン -> フェードアウト
        public void FadeInOut()
        {
            m_fadeInOut = true;
        }


        //終了判断用
        void CallBack()
        {
            m_isCallBack = true;
        }
        public bool CallBack(int a)
        {
            return m_isCallBack;
        }
    }
}
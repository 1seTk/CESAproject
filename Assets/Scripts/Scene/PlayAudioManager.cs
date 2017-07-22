using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YamagenLib
{
    public class PlayAudioManager : MonoBehaviour
    {
        // シングルトン
        static public PlayAudioManager instance;

        PlayStage m_loadStage;
        PlayBGM m_loadBGM;

        enum PlayBGM
        {
            NONE = 0,
            Play_01,
            Play_02,
            Play_03,
            Play_04,
            Play_05,
            length
        }


        /// <summary>
        /// 初期化
        /// </summary>
        private void Awake()
        {
            // シングルトン
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        // Use this for initialization
        void Start()
        {
            m_loadStage = (PlayStage)System.Enum.GetValues(typeof(PlayStage)).Length;

            m_loadBGM = PlayBGM.NONE;
        }

        // Update is called once per frame
        void Update()
        {
            PlayStage nowStage = PlayInstructor.instance.GetLoadStage();
            if (nowStage != m_loadStage)
            {
                // シーンが変わったら
                // シーンを記憶
                m_loadStage = nowStage;

                PlayBGM nowBGM = m_loadBGM;

                switch (m_loadStage)
                {
                    case PlayStage.Stage1:
                    case PlayStage.Stage2:
                    case PlayStage.Stage3:
                    case PlayStage.Stage4:
                    case PlayStage.Stage5:
                        AudioManager.Instance.AudioSet(PlayBGM.Play_01.ToString(), "MP3\\1to5");
                        AudioManager.Instance.SetOption(PlayBGM.Play_01.ToString(), true, false);
                        m_loadBGM = PlayBGM.Play_01;
                        break;
                    case PlayStage.Stage6:
                    case PlayStage.Stage7:
                    case PlayStage.Stage8:
                    case PlayStage.Stage9:
                    case PlayStage.Stage10:
                        AudioManager.Instance.AudioSet(PlayBGM.Play_02.ToString(), "MP3\\6to10");
                        AudioManager.Instance.SetOption(PlayBGM.Play_02.ToString(), true, false);
                        m_loadBGM = PlayBGM.Play_02;
                        break;
                    case PlayStage.Stage11:
                    case PlayStage.Stage12:
                    case PlayStage.Stage13:
                    case PlayStage.Stage14:
                    case PlayStage.Stage15:
                        AudioManager.Instance.AudioSet(PlayBGM.Play_03.ToString(), "MP3\\11to15");
                        AudioManager.Instance.SetOption(PlayBGM.Play_03.ToString(), true, false);
                        m_loadBGM = PlayBGM.Play_03;
                        break;
                    case PlayStage.Stage16:
                    case PlayStage.Stage17:
                    case PlayStage.Stage18:
                    case PlayStage.Stage19:
                        AudioManager.Instance.AudioSet(PlayBGM.Play_04.ToString(), "MP3\\16to19");
                        AudioManager.Instance.SetOption(PlayBGM.Play_04.ToString(), true, false);
                        m_loadBGM = PlayBGM.Play_04;
                        break;
                    case PlayStage.Stage20:
                        AudioManager.Instance.AudioSet(PlayBGM.Play_05.ToString(), "MP3\\20");
                        AudioManager.Instance.SetOption(PlayBGM.Play_05.ToString(), true, false);
                        m_loadBGM = PlayBGM.Play_05;
                        break;
                    default:
                        m_loadBGM = PlayBGM.NONE;
                        break;
                }

                if ((m_loadBGM != nowBGM) && (m_loadBGM != PlayBGM.NONE))
                {
                    if (nowBGM != PlayBGM.NONE) 
                        StartCoroutine(AudioManager.Instance.fadeOut(nowBGM.ToString(), 3.0f));
                    StartCoroutine(AudioManager.Instance.fadeIn(m_loadBGM.ToString(), 3.0f, 1.5f));
                }
            }
        }

        public void Stop()
        {
            AudioManager.Instance.Stop(m_loadBGM.ToString());
        }
    }
}
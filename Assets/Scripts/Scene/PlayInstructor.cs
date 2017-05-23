using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  //シーンの管理用

namespace YamagenLib
{
    /// <summary>
    /// ステージの列挙
    /// </summary>
    public enum PlayStage
    {
        Stage1,
        Stage2,
        Stage3,
        Stage4,
        Stage5,
        Stage6,
        Stage7,
        Stage8
    }

    /// <summary>
    /// プレイシーンの管理するクラス
    /// </summary>
    public class PlayInstructor : MonoBehaviour
    {
        // シングルトン
        static public PlayInstructor instance;

        [SerializeField]
        // 初期ステージ
        PlayStage m_initScene = PlayStage.Stage1;

        // ロードされてるステージ
        PlayStage m_loadStage;

        bool m_initFlag = true;

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
        void Start() // ScenemanagerはStart以降じゃないと危ないため
        {
            // 初期ステージをロード
            LoadStage(m_initScene);
        }

        // ステージをロードする
        public void LoadStage(PlayStage stage)
        {
            if (m_initFlag == false)
                SceneManager.UnloadSceneAsync(m_loadStage.ToString());
            else
                m_initFlag = false;
            SceneManager.LoadScene(stage.ToString(), LoadSceneMode.Additive);
            m_loadStage = stage;
        }

        // ステージをリロードする
        public void ReLoadStage()
        {
            SceneManager.UnloadSceneAsync(m_loadStage.ToString());
            SceneManager.LoadScene(m_loadStage.ToString(), LoadSceneMode.Additive);
        }

    }
}
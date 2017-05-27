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
        Stage8,
        Stage9
    }

    /// <summary>
    /// プレイシーンの管理するクラス
    /// </summary>
    public class PlayInstructor : MonoBehaviour
    {
        // シングルトン
        static public PlayInstructor instance;

        // 初期ステージ
        static public PlayStage m_nextScene = PlayStage.Stage1;

        // ロードされてるステージ
        private PlayStage m_loadStage;

        private bool m_initFlag = true;

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
            LoadStage(m_nextScene);
        }

        // ステージをロードする
        public void LoadStage(PlayStage stage)
        {
            if (m_initFlag == false){
                ShunLib.GameOverDirection.instance.Initialize();    // オーバーの初期化
                ShunLib.ClearDirection.instance.Initialize();    // クリアの初期化
                SceneManager.UnloadSceneAsync(m_loadStage.ToString());
            }
            else{
                m_initFlag = false;
            }
            SceneManager.LoadScene(stage.ToString(), LoadSceneMode.Additive);
            m_loadStage = stage;
        }

        // ステージをリロードする
        public void ReLoadStage()
        {
            ShunLib.GameOverDirection.instance.Initialize();    // オーバーの初期化
            ShunLib.ClearDirection.instance.Initialize();    // クリアの初期化
            SceneManager.UnloadSceneAsync(m_loadStage.ToString());
            SceneManager.LoadScene(m_loadStage.ToString(), LoadSceneMode.Additive);
        }

        public PlayStage GetLoadStage()
        {
            return m_loadStage;
        }

        public void ChangenextScene()
        {
            SceneManager.UnloadSceneAsync(m_loadStage.ToString());
            LoadStage(m_loadStage);
        }

        /// <summary>
        /// ステージをアンロード
        /// </summary>
        public void StageUnLoad()
        {
            SceneManager.UnloadSceneAsync(m_loadStage.ToString());
        }
    }
}
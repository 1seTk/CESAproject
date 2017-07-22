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
        Stage9,
        Stage10,
        Stage11,
        Stage12,
        Stage13,
        Stage14,
        Stage15,
        Stage16,
        Stage17,
        Stage18,
        Stage19,
        Stage20
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
        private int m_loadStage = -1;

        private bool m_initFlag = true;

		// 前回のステージ
		private int m_lastStage = -1;

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
                SceneManager.UnloadSceneAsync(((PlayStage)m_loadStage).ToString());
            }
            else{
                m_initFlag = false;
            }
            SceneManager.LoadScene(stage.ToString(), LoadSceneMode.Additive);
            m_lastStage = m_loadStage;
            m_loadStage = (int)stage;
            SelectManager.SceneSave((int)stage);
        }

        // ステージをリロードする
        public void ReLoadStage()
        {
            ShunLib.GameOverDirection.instance.Initialize();    // オーバーの初期化
            ShunLib.ClearDirection.instance.Initialize();    // クリアの初期化
            SceneManager.UnloadSceneAsync(( (PlayStage)m_loadStage ).ToString());
            SceneManager.LoadScene(((PlayStage)m_loadStage).ToString(), LoadSceneMode.Additive);
			m_lastStage = (int)m_loadStage;
		}

		public PlayStage GetLoadStage()
        {
            return (PlayStage)m_loadStage;
        }

        public void ChangenextScene()
        {
            SceneManager.UnloadSceneAsync(((PlayStage)m_loadStage).ToString());
            LoadStage((PlayStage)m_loadStage);
        }

        /// <summary>
        /// ステージをアンロード
        /// </summary>
        public void StageUnLoad()
        {
            PlayAudioManager.instance.Stop();
            SceneManager.UnloadSceneAsync(((PlayStage)m_loadStage).ToString());
        }

		/// <summary>
		/// リロードしたか？
		/// </summary>
		public bool IsReload()
		{
			return (int)m_loadStage == m_lastStage;
		}
    }
}
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
        [SerializeField]
        // 初期シーン
        PlayStage m_initScene = PlayStage.Stage1;

        // ロードされてるシーン
        Scene m_loadStage;

        bool m_initFlag = true;

        /// <summary>
        /// 初期化
        /// </summary>
        void Start() // ScenemanagerはStart以降じゃないと危ないため
        {
            // 初期ステージをロード
            LoadStage(m_initScene);
        }

        // ステージをロードする
        public void LoadStage(PlayStage stage)
        {
            if (m_initFlag == false)
                SceneManager.UnloadSceneAsync(m_loadStage.name);
            else
                m_initFlag = false;
            SceneManager.LoadScene(stage.ToString(), LoadSceneMode.Additive);
            m_loadStage = SceneManager.GetSceneByName(stage.ToString());

        }

        // ステージをリロードする
        public void ReLoadStage()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  //シーンの管理用

// 山元のやーつ
namespace YamagenLib
{
    // シーンのの列挙(シーン名と同じ)
    public enum GameScene
    {
        Title,
        Select,
        Play,
        Clear,
        Over
    }

    // シーンに指示出すクラス
    public class SceneInstructor : MonoBehaviour
    {
        [SerializeField]
        // 初期シーン
        GameScene m_initScene=GameScene.Title;

        // ロードされてるシーン
        Scene m_loadScene;

        bool m_initFlag = true;

        /// <summary>
        /// 初期化
        /// </summary>
        void Start() // ScenemanagerはStart以降じゃないと危ないため
        {
            // 初期シーンをロード
            LoadMainScene(m_initScene);
        }

        // メインのシーンをロードする
        public void LoadMainScene(GameScene scene)
        {
            if (m_initFlag == false)
                SceneManager.UnloadSceneAsync(m_loadScene.name);
            else
                m_initFlag = false;
            SceneManager.LoadScene(scene.ToString(), LoadSceneMode.Additive);
            m_loadScene = SceneManager.GetSceneByName(scene.ToString());

        }
    }
}
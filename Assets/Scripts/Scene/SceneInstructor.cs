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
        // シングルトン
        static public SceneInstructor instance;

        [SerializeField]
        // 初期シーン
        GameScene m_initScene=GameScene.Title;

        // ロードされてるシーン
        GameScene m_loadScene;

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
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        void Start() // ScenemanagerはStart以降じゃないと危ないため
        {
            // 初期シーンをロード
            LoadMainScene(m_initScene);
        }

        /// <summary>
        /// メインシーンのロード
        /// </summary>
        /// <param name="scene">ロードしたいシーン</param>
        public void LoadMainScene(GameScene scene)
        {
            if (m_initFlag == false)
                // 今のシーンをアンロード
                SceneManager.UnloadSceneAsync(m_loadScene.ToString());
            else
                m_initFlag = false;
            // 次のシーンをロード
            SceneManager.LoadScene(scene.ToString(), LoadSceneMode.Additive);
            // 変わったシーンを覚えておく
            m_loadScene = scene;

            // シーンがタイトル画面かセレクト画面の時SelectManagerを作成
            if (scene==GameScene.Title||scene==GameScene.Select){
                SelectManager.instance = new SelectManager();
            }
            else{
                // それ以外の時は破棄
                Destroy(SelectManager.instance.gameObject);
            }
        }
    }
}
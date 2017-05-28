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
        Endless,
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
        private GameScene m_initScene = GameScene.Title;

        // ロードされてるシーン
        private GameScene m_loadScene;
        private GameScene m_nextLoadScene;
        private GameScene m_oldLoadScene;
        private bool m_initFlag = true;
        private bool m_isChange = false;

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
            m_loadScene = m_initScene;
            m_nextLoadScene = m_loadScene;
            m_oldLoadScene = m_initScene;
        }

        /// <summary>
        /// 更新
        /// </summary>
        private void Update()
        {
            // シーンの変更があった時変更
            if (m_loadScene != m_nextLoadScene)
            {
                StartCoroutine("SceneLoad");
            }
        }

        /// <summary>
        /// メインシーンのロード
        /// </summary>
        /// <param name="scene">ロードしたいシーン</param>
        public void LoadMainScene(GameScene scene)
        {
            if (m_initFlag ) SceneManager.LoadScene(m_initScene.ToString(), LoadSceneMode.Additive);

            if (m_isChange == false)
            {
                // 次のシーンを変える
                m_nextLoadScene = scene;
            }
            m_initFlag = false;
        }

        // コルーチン  
        private IEnumerator SceneLoad()
        {
            // 変更開始
            m_isChange = true;
            // コルーチンの処理  
            // 変わったシーンを覚えておく
            m_oldLoadScene = m_loadScene;
            // シーンを変更
            m_loadScene = m_nextLoadScene;
            // フェードイン
            ShunLib.FadeScene.instance.FadeIn();
            // 1秒待つ  
            yield return new WaitForSeconds(1.0f);
            // 今のシーンをアンロード
            SceneManager.UnloadSceneAsync(m_oldLoadScene.ToString());
            // 1秒待つ  
            yield return new WaitForSeconds(0.7f);
            // 次のシーンをロード
            SceneManager.LoadScene(m_nextLoadScene.ToString(), LoadSceneMode.Additive);
            // フェードアウト
            ShunLib.FadeScene.instance.FadeOut();
            // 1秒待つ  
            yield return new WaitForSeconds(1.0f);
            // 変更終了
            m_isChange = false;
        }

        public GameScene GetLoadScene()
        {
            return m_loadScene;
        }
    }
}
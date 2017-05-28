using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   // ボタン用

// 山元のやーつ
namespace YamagenLib
{
    // シーン遷移のボタン用クラス
    public class SceneButton : MonoBehaviour
    {
        // ロードしたいシーン
        [SerializeField]
        public GameScene m_scene = GameScene.Title;

        // 初期化
        void Awake()
        {
            GetComponent<Button>().onClick.AddListener(LoadScene);
        }

        // シーンをロードする
        public void LoadScene()
        {
            AudioManager.Instance.Play("SELECTCUBE");
            SceneInstructor.instance.LoadMainScene(m_scene);
        }
    }
}
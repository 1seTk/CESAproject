using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   // ボタン用

// 山元のやーつ
namespace YamagenLib
{
    // シーン遷移のボタン用クラス
    public class StageButton : MonoBehaviour
    {
        // ロードしたいシーン
        public PlayStage m_stage = PlayStage.Stage1;

        // 初期化
        void Awake()
        {
            GetComponent<Button>().onClick.AddListener(LoadScene);
        }

        // シーンをロードする
        public void LoadScene()
        {
            // プレイインストラクターにシーンを設定
            PlayInstructor.m_nextScene = m_stage;
            // 次のシーンに移動
            SceneInstructor.instance.LoadMainScene(GameScene.Play);
        }
    }
}
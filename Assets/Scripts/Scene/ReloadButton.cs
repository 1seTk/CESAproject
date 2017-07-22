using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   // ボタン用
using UnityEngine.SceneManagement;  //シーンの管理用

// 山元のやーつ
namespace YamagenLib
{
    // ステージリロードのボタン用クラス
    public class ReloadButton : MonoBehaviour
    {
        // 初期化
        void Awake()
        {
            GetComponent<Button>().onClick.AddListener(ReLoadScene);
        }

        // シーンをリロードする
        public void ReLoadScene()
        {
            PlayInstructor.instance.ReLoadStage();
        }
    }
}
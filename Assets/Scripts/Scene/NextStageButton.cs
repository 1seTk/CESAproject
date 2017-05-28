using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace YamagenLib
{
    public class NextStageButton : MonoBehaviour
    {
        // 初期化
        void Start()
        {
            GetComponent<Button>().onClick.AddListener(ChangeNextScene);
        }

        private void Update()
        {
            Debug.Log("今のステージ"+(int)PlayInstructor.instance.GetLoadStage());
            Debug.Log("ステージの量"+System.Enum.GetValues(typeof(PlayStage)).Length);
            if ((int)PlayInstructor.instance.GetLoadStage()+1 >= System.Enum.GetValues(typeof(PlayStage)).Length) Destroy(this.gameObject);
        }

        // 次のシーンを呼び出す
        public void ChangeNextScene()
        {
            AudioManager.Instance.Play("SELECTCUBE");
            PlayInstructor.instance.LoadStage(PlayInstructor.instance.GetLoadStage() + 1);
        }
    }
}

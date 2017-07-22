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
            if ((int)PlayInstructor.instance.GetLoadStage() + 3 > System.Enum.GetValues(typeof(PlayStage)).Length) Destroy(this.gameObject);
            else GetComponent<Button>().onClick.AddListener(ChangeNextScene);
        }

        // 次のシーンを呼び出す
        public void ChangeNextScene()
        {
            PlayInstructor.instance.LoadStage(PlayInstructor.instance.GetLoadStage() + 1);
        }
    }
}

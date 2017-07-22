using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YamagenLib
{
    public class MainAudioManager : MonoBehaviour
    {
        public void Initialize()
        {
            AudioManager.Instance.AudioSet("MainBGM", "MP3\\bgm");
            AudioManager.Instance.SetOption("MainBGM", true, false);
            AudioManager.Instance.Play("MainBGM");
        }

        public void AudioChange(GameScene scene)
        {
            if (scene == GameScene.Play)
            {   // 止める
                StartCoroutine(AudioManager.Instance.fadeOut("MainBGM", 3.0f));
            }
            else if (AudioManager.Instance.IsPlaying("MainBGM") == false)
            {
                // 流す
                StartCoroutine(AudioManager.Instance.fadeIn("MainBGM", 3.0f, 0.0f));
            }
        }
    }
}
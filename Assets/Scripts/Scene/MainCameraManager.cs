using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace YamagenLib
{
    public class MainCameraManager : MonoBehaviour
    {
        public void Initialize()
        {
        }

        public void CameraChange(GameScene scene,GameObject camera)
        {
            //if (scene == GameScene.Play) camera.SetActive(false);
            //else camera.SetActive(true);
        }
    }
}
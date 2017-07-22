using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YamagenLib
{
    public class MainManager : MonoBehaviour
    {
        MainAudioManager m_audioManager;
        MainCameraManager m_cameraManager;

        GameScene m_loadScene;

        [SerializeField]
        GameObject m_camera;

        private void Awake()
        {
            m_audioManager = this.gameObject.AddComponent<MainAudioManager>();
            m_cameraManager = this.gameObject.AddComponent<MainCameraManager>();
        }

        // Use this for initialization
        void Start()
        {
            m_loadScene = SceneInstructor.instance.GetLoadScene();
            m_audioManager.Initialize();
        }

        // Update is called once per frame
        void Update()
        {
            GameScene nowScene = SceneInstructor.instance.GetLoadScene();
            if (nowScene!=m_loadScene)
            {
                m_loadScene = nowScene;
                m_audioManager.AudioChange(m_loadScene);
            }
            m_cameraManager.CameraChange(m_loadScene, m_camera);
        }
    }
}
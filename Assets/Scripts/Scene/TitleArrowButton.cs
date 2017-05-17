using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace YamagenLib
{
    public class TitleArrowButton : MonoBehaviour
    {
        // 方向列挙
        public enum Direction
        {
            Right,
            Left
        }

        [SerializeField]
        private Direction m_dir = Direction.Right;

        // 初期化
        void Awake()
        {
            GetComponent<Button>().onClick.AddListener(CubeRotate);
        }

        // cube回転
        public void CubeRotate()
        {
            if (m_dir == Direction.Right)
            {
                TitleManager.instance.ObjectRotate(1.0f);
            }
            else
            {
                TitleManager.instance.ObjectRotate(-1.0f);
            }
        }
    }
}

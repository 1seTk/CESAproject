using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YamagenLib
{

    public class TitleManager : MonoBehaviour
    {
        // シングルトン
        static public TitleManager instance;

        // オブジェクト
        [SerializeField, Space(10)]
        private GameObject m_obj;


        // タッチマウス用
        private Vector3 m_startPos;           // タッチ開始座標
        private Vector3 m_endPos;             // タッチ終了座標
        private float m_screenWidth;        // 画面の横幅

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            // マウスが押された時
            if (Input.GetMouseButtonDown(0))
            {
                m_startPos = Input.mousePosition;             // プッシュ開始座標
            }
            // マウスが離された時
            if (Input.GetMouseButtonUp(0))
            {
                // マウスが離れた座標
                m_endPos = Input.mousePosition;
                // クリック開始位置と終了位置がオブジェクト内の場合シーンを変更
                if (SearchObject(m_startPos) && SearchObject(m_endPos))
                {
                    // 次のシーンに移動
                    SceneInstructor.instance.LoadMainScene(GameScene.Select);
                }

            }

        }

        /// <summary>
        /// 指定座標にオブジェクトがあるかの確認
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        private bool SearchObject(Vector3 pos)
        {
            bool isSearch = false;
            GameObject result = null;
            // 指定場所のオブジェクトを取得
            Ray ray = Camera.main.ScreenPointToRay(pos);
            Debug.DrawRay(ray.origin, ray.direction, Color.red, 10.0f);
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(ray, out hit))
            {
                result = hit.collider.gameObject;
            }

            // セットされているオブジェクトと同じ時
            if (result == m_obj) isSearch = true;

            return isSearch;
        }
    }
}
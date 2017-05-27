using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YamagenLib
{
    public class Enter
    {
        // タッチマウス用
        private Vector3 m_startPos;           // タッチ開始座標
        private Vector3 m_endPos;             // タッチ終了座標
        private float m_screenWidth;        // 画面の横幅

        public virtual void SceneChange(int select) { }

        // Use this for initialization
        public void Initialize() {
            // 画面の横幅
            m_screenWidth = Screen.width;
        }

        // Update is called once per frame
        public void Change(int select,Vector3 startPos, Vector3 endPos) {
            // クリック開始位置と終了位置がオブジェクト内の場合シーンを変更
            if (SearchObject(startPos) && SearchObject(endPos))
            {
                SceneChange(select);
                Debug.Log("change！");
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

            // セレクトマネージャーのセットされているオブジェクトと同じ時
            if (result == SelectManager.instance.GetSetingObject()) isSearch = true;

            return isSearch;
        }
    }
}

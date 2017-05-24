using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;  // どついーん使用

namespace YamagenLib
{
    public class SelectManager : MonoBehaviour
    {
        // シングルトン
        static public SelectManager instance;

        // 次のシーン
        [SerializeField]
        GameScene m_nextScene;

        [SerializeField, Header("オブジェクトに設定する画像（ステージ数まで）")]
        Texture[] m_texture;

        // 回転させるオブジェクト
        [SerializeField, Space(10)]
        private GameObject m_obj;

        // 回転時間
        [SerializeField]
        private float m_rotateTime = 1.0f;

        // タッチマウス用
        private Vector3 m_startPos;           // タッチ開始座標
        private Vector3 m_endPos;             // タッチ終了座標
        private float m_screenWidth;        // 画面の横幅

        // スワイプ許容割合
        [SerializeField, Range(0.0f, 1.0f)]
        private float m_AcceptableValue;
        private float m_MoveTouchX;         // 横軸移動量
        private float m_MoveMouseX;         // 横軸移動量

        // 回転tween
        private Tweener m_rotation;         // 回転

        // 現在背面の面
        private SelectCube.Face m_backFace = SelectCube.Face.BACK;
        private int m_backTextureNum = 0;

        private SelectCube.Face m_leftFace = SelectCube.Face.LEFT;
        private int m_leftTextureNum = 0;

        private int m_selectScene = 0;

        /// <summary>
        /// 初期化
        /// </summary>
        void Awake()
        {
            // シングルトン
            if (instance == null) instance = this;
            else Destroy(gameObject);

            // 画面の横幅
            m_screenWidth = Screen.width;

            // ついーん初期化
            DOTween.Init();

            // キューブの画像初期化
            InitializeCubeTexture();
        }

        /// <summary>
        /// 更新
        /// </summary>
        void Update()
        {
            TouchUpdate();
            MouseUpdate();
            
        }

        /// <summary>
        /// タッチの場合
        /// </summary>
        private void TouchUpdate()
        {
            // 一か所タッチされている時
            if (Input.touchCount == 1)
            {
                // タッチの状態検出
                Touch touch = Input.GetTouch(0);
                switch (touch.phase)
                {
                    case TouchPhase.Began:      // タッチ開始時
                        // タッチ開始座標
                        m_startPos = touch.position;
                        break;
                    case TouchPhase.Moved:      // タッチ移動時
                    case TouchPhase.Stationary: // タッチ静止時
                        break;
                    case TouchPhase.Ended:      // タッチ終了時
                        // タッチ終了座標
                        m_endPos = touch.position;

                        // タッチ開始位置と終了位置がオブジェクト内の場合シーンを変更
                        if (SearchObject(m_startPos) && SearchObject(m_endPos)) SceneChange();

                        // 横移動量画面割合(-1<tx<1)
                        m_MoveTouchX = (touch.position.x - m_startPos.x) / m_screenWidth;
                        if ((m_MoveTouchX > m_AcceptableValue) || (m_MoveTouchX < -m_AcceptableValue))
                        {
                            // スワイプが許容内の場合
                            // 回転
                            ObjectRotate(m_MoveTouchX);
                        }
                        // 移動量リセット
                        m_MoveTouchX = 0.0f;
                        break;
                    case TouchPhase.Canceled:   // タッチキャンセル時
                        break;
                    default:                    // その他  
                        break;
                }
            }
        }

        /// <summary>
        /// マウスの場合
        /// </summary>
        private void MouseUpdate()
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
                if (SearchObject(m_startPos) && SearchObject(m_endPos)) SceneChange();

                // 横移動量画面割合(-1<tx<1)
                m_MoveMouseX = (Input.mousePosition.x - m_startPos.x) / m_screenWidth;

                if ((m_MoveMouseX > m_AcceptableValue) || (m_MoveMouseX < -m_AcceptableValue))
                {
                    // 許容値内でスワイプの場合

                    // 回転
                    ObjectRotate(m_MoveMouseX);
                }
                m_MoveMouseX = 0.0f;                       // 移動量リセット
            }
        }

        /// <summary>
        /// オブジェクトの回転
        /// </summary>
        /// <param name="moveDir">移動割合</param>
        public void ObjectRotate(float moveDir)
        {
            // 回転
            if (m_rotation == null)
            {
                if ((moveDir < 0) && (m_selectScene < m_texture.Length - 1)) RotateTexture(true);   // 右
                else if ((moveDir > 0) && (m_selectScene > 0)) RotateTexture(false);                // 左
                else return;

                // 1か-1にする
                moveDir /= Mathf.Abs(moveDir);

                m_rotation =
                    m_obj.transform.DORotate(
                    new Vector3(0, m_obj.transform.eulerAngles.y + (-90 * moveDir), 0),    // 終了時点のRotation
                    m_rotateTime                        // アニメーション時間
                ).OnKill(() => m_rotation = null);
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

            if (result == m_obj) isSearch = true;

            return isSearch;
        }

        /// <summary>
        /// シーンを変更
        /// </summary>
        private void SceneChange()
        {
            // 次のシーンに移動
            SceneInstructor.instance.LoadMainScene(m_nextScene);
        }

        /// <summary>
        /// cube画像の初期設定
        /// </summary>
        private void InitializeCubeTexture()
        {
            m_obj.GetComponent<SelectCube>().SetInitTexture(m_texture);
            m_backFace = SelectCube.Face.BACK;
            m_leftFace = SelectCube.Face.LEFT;
            m_backTextureNum = (int)m_backFace;
            m_leftTextureNum = (int)m_leftFace;
            Debug.Log("左面は" + m_leftFace + "で画像は " + m_leftTextureNum);
            Debug.Log("背面は" + m_backFace + "で画像は " + m_backTextureNum);
        }

        /// <summary>
        /// テクスチャの変更
        /// </summary>
        /// <param name="dir"></param>
        private void RotateTexture(bool dir)
        {
            if (dir)
            {// 右移動
                // Leftが正面の時は左にする
                if (m_leftFace == SelectCube.Face.LEFT) m_leftFace = SelectCube.Face.FRONT;
                else m_leftFace++;   // その他はマイナス
                if (m_backFace == SelectCube.Face.LEFT) m_backFace = SelectCube.Face.FRONT;
                else m_backFace++;

                // 画像番号処理
                MoveTextureNum(1);

                // 背面の画像を変更
                m_obj.GetComponent<SelectCube>().ChangeTexture(m_backFace, m_texture[m_backTextureNum]);

                Debug.Log("右移動");
            }
            else
            {// 左移動
                
             // frontの時は一周回ったことになりleftへ
                if (m_leftFace == SelectCube.Face.FRONT) m_leftFace = SelectCube.Face.LEFT;
                else m_leftFace--;
                if (m_backFace == SelectCube.Face.FRONT) m_backFace = SelectCube.Face.LEFT;
                else m_backFace--;

                // 番号処理
                MoveTextureNum(-1);

                // 左面の画像を変更
                m_obj.GetComponent<SelectCube>().ChangeTexture(m_leftFace, m_texture[m_leftTextureNum]);

                Debug.Log("左移動");
                
            }
            Debug.Log(m_selectScene);
        }

        /// <summary>
        /// 画像の番号処理
        /// </summary>
        /// <param name="num">移動数</param>
        private void MoveTextureNum(int num)
        {
            // 背面画像の番号処理
            if ((m_backTextureNum + num) > m_texture.Length - 1) m_backTextureNum -= 3;
            else if ((m_backTextureNum + num) < 0) m_backTextureNum += 3;
            else m_backTextureNum += num;

            // 左面画像の番号処理
            if ((m_leftTextureNum + num) > m_texture.Length - 1) m_leftTextureNum -= 3;
            else if ((m_leftTextureNum + num) < 0) m_leftTextureNum += 3;
            else if (num < 0) { m_leftTextureNum += num; }
            else m_leftTextureNum += num;

            m_selectScene += num;
        }
    }
}

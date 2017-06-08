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

        [SerializeField, Header("オブジェクトに設定する画像（ステージ数まで）")]
        Texture[] m_texture;

        // シーンを移動する用のScript
        Enter m_enterScript;

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
        [SerializeField, Range(0.0f, 1.0f)]        private float m_AcceptableValue;

        private float m_MoveTouchX;         // 横軸移動量
        private float m_MoveMouseX;         // 横軸移動量

        // 回転tween
        private Tweener m_rotation;         // 回転

        // 現在背面の面
        private SelectCube.Face m_backFace = SelectCube.Face.BACK;
        private int m_backTextureNum = 0;

        private SelectCube.Face m_leftFace = SelectCube.Face.LEFT;
        private int m_leftTextureNum = 0;

        // 選択されている数
        private int m_selectScene;
        static private int m_saveScene = 0;
        /// <summary>
        /// 初期化
        /// </summary>
        void Awake()
        {
            // シングルトン
            if (instance == null)
            {
                instance = this;
                m_selectScene = m_saveScene;
            }
            else Destroy(gameObject);

            // 画面の横幅
            m_screenWidth = Screen.width;

            // ついーん初期化
            DOTween.Init();

            // キューブの画像初期化
            InitializeCubeTexture();

            this.Initialize();
        }

        public void Initialize()
        {
            // シーンによってスクリプトを変更
            switch (SceneInstructor.instance.GetLoadScene())
            {
                case GameScene.Title:
                    m_enterScript = new TitleEnter();
                    break;
                case GameScene.Select:
                    m_enterScript = new SelectEnter();
                    break;
                default:
                    Debug.Log("あかんよ");
                    break;
            }
            m_enterScript.Initialize();
        }

        /// <summary>
        /// 更新
        /// </summary>
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

                // 横移動量画面割合(-1<tx<1)
                m_MoveMouseX = (Input.mousePosition.x - m_startPos.x) / m_screenWidth;

                if ((m_MoveMouseX > m_AcceptableValue) || (m_MoveMouseX < -m_AcceptableValue)){
                    // 許容値内でスワイプの場合
                    // 回転
                    ObjectRotate(m_MoveMouseX);
                }
                else {
                    // シーン遷移の判定
                    m_enterScript.Change(m_selectScene, m_startPos, m_endPos);
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
            if ((m_rotation == null))
            {
                if ((moveDir < 0)) RotateTexture(true);   // 右
                else if ((moveDir > 0)) RotateTexture(false);                // 左
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
        /// cube画像の初期設定
        /// </summary>
        private void InitializeCubeTexture()
        {
            m_obj.GetComponent<SelectCube>().SetInitTexture(m_texture);
            m_backFace = SelectCube.Face.BACK;
            m_leftFace = SelectCube.Face.LEFT;
            m_backTextureNum = (int)m_backFace;
            m_leftTextureNum = (int)m_leftFace;
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
                m_selectScene++;
                if (m_selectScene > m_texture.Length-1) m_selectScene = 0; 
                m_backTextureNum = m_selectScene + 2;

                // 背面の画像を変更
                if (m_backTextureNum > 0 && m_backTextureNum < m_texture.Length)
                    m_obj.GetComponent<SelectCube>().ChangeTexture(m_backFace, m_texture[m_backTextureNum]);
            }
            else
            {// 左移動
                // frontの時は一周回ったことになりleftへ
                if (m_leftFace == SelectCube.Face.FRONT) m_leftFace = SelectCube.Face.LEFT;
                else m_leftFace--;
                if (m_backFace == SelectCube.Face.FRONT) m_backFace = SelectCube.Face.LEFT;
                else m_backFace--;

                // 番号処理
                m_selectScene--;
                if (m_selectScene < 0) m_selectScene = m_texture.Length - 1;
                m_leftTextureNum = m_selectScene - 1;

                // 左面の画像を変更
                if (m_leftTextureNum >= 0 && m_leftTextureNum < m_texture.Length) 
                    m_obj.GetComponent<SelectCube>().ChangeTexture(m_leftFace, m_texture[m_leftTextureNum]);                
            }
            m_saveScene = m_selectScene;
        }

        public GameObject GetSetingObject() { return m_obj; }
        public PlayStage GetSelectStage() { return (PlayStage)m_selectScene; }

    }
}

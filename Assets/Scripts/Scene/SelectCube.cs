using System.Collections;
using System.Collections.Generic;
using System.Linq;   // 子要素取得の際使用
using UnityEngine;
using DG.Tweening;   // どついーん使用

// 山元のやーつ
namespace YamagenLib
{
    public class SelectCube : MonoBehaviour
    {
        public enum Face
        {
            FRONT = 0,
            RIGHT = 1,
            BACK = 2,
            LEFT = 3
        }

        [SerializeField]
        private float m_moveDistance = 0.0f;

        [SerializeField]
        private float m_animeTime = 0.0f;

        [SerializeField]
        private Ease m_easeType = Ease.InSine;

        private Vector3 m_startPos;
        private Vector3 m_endPos;


        // ぷかぷか
        private Sequence m_moveSequence;

        // 画像
        private MeshRenderer[] m_face = new MeshRenderer[4];

        /// <summary>
        /// 初期化
        /// </summary>
        void Awake()
        {
            // 初期座標設定
            Vector3 pos = transform.position;
            transform.position = new Vector3(pos.x, -(m_moveDistance / 2.0f), pos.z);
            pos = transform.position;

            // ループぷかぷか
            m_moveSequence = DOTween.Sequence();
            m_moveSequence.Append(transform.DOLocalMoveY(m_moveDistance / 2.0f, m_animeTime)).SetEase(m_easeType);
            m_moveSequence.Append(transform.DOLocalMoveY(pos.y, m_animeTime * 1.7f)).SetEase(m_easeType);
            m_moveSequence.Append(transform.DOShakePosition(1.0f, new Vector3(10.0f, 0, 0), 7, 0));
            m_moveSequence.SetLoops(-1);

        }

        public void Update()
        {
        }

        /// <summary>
        /// 移動一時停止
        /// </summary>
        public void MoveStop()
        {
            // 一時停止
            m_moveSequence.Pause();
        }

        /// <summary>
        /// 移動再開
        /// </summary>
        public void MoveReStart()
        {
            // 一時停止
            m_moveSequence.Play();
        }

        /// <summary>
        /// テクスチャの初期設定
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        public void SetInitTexture(Texture[] txt)
        {
            int i = (int)SelectManager.instance.GetSelectStage();
            int cnt = 0;
            //子のマテリアルに設定する
            foreach (Transform childTransform in this.gameObject.transform)
            {
                if (i >= txt.Length) i = 0;

                m_face[cnt] = childTransform.gameObject.GetComponent<MeshRenderer>();
                if (cnt == 3)
                {
                    if (i <= 3) m_face[cnt].material.mainTexture = txt[txt.Length - (4 - i)];
                    else m_face[cnt].material.mainTexture = txt[i - 4];
                }
                else m_face[cnt].material.mainTexture = txt[i];
                i++;
                cnt++;
            }
        }

        /// <summary>
        /// 指定された面の画像変更
        /// </summary>
        /// <param name="face">面</param>
        /// <param name="txt">変更する画像</param>
        public void ChangeTexture(Face face, Texture txt)
        {
            if(SceneInstructor.instance.GetLoadScene()==GameScene.Select)
            m_face[(int)face].material.mainTexture = txt;
        }
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;   // どついーん使用

// 山元のやーつ
namespace YamagenLib
{
    public class TitleCube : MonoBehaviour
    {
        [SerializeField]
        private float m_moveDistance = 0.0f;

        [SerializeField]
        private float m_animeTime=0.0f;

        [SerializeField]
        private Ease m_easeType = Ease.InSine;

        // ぷかぷか
        private Sequence m_moveSequence;

        /// <summary>
        /// 初期化
        /// </summary>
        void Awake()
        {
            // 初期座標設定
            Vector3 pos = transform.position;
            transform.position = new Vector3(pos.x, -(m_moveDistance / 2.0f), pos.z);
            pos = transform.position;

            // どついーん初期化
            DOTween.Init(false, true, LogBehaviour.ErrorsOnly);

            // ループぷかぷか
            m_moveSequence = DOTween.Sequence();
            m_moveSequence.Append(transform.DOLocalMoveY(m_moveDistance / 2.0f, m_animeTime)).SetEase(m_easeType);
            m_moveSequence.Append(transform.DOLocalMoveY(pos.y, m_animeTime*1.7f)).SetEase(m_easeType);
            m_moveSequence.SetLoops(-1);
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
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class NewText : MonoBehaviour {

    private Sequence m_moveSequence;

    private void Awake()
    {
        transform.DOScale(0.0f, 1.0f).SetLoops(-1);
        m_moveSequence = DOTween.Sequence();
        m_moveSequence.Append(transform.DOScale(1.0f, 0.5f));
        m_moveSequence.Append(transform.DOScale(3.0f, 0.5f));
        m_moveSequence.SetLoops(-1);
    }
}

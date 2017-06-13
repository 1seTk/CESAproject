using UniRx;
using UnityEngine;
using System.Collections;

public class PlayerFall : MonoBehaviour
{

    [SerializeField, Range(0, -10), Tooltip("落下許容範囲")]
    private float m_fall = -2.0f;

    private PlayerCore m_core;

    void Start()
    {
        m_core = GetComponent<PlayerCore>();
    }


    // Update is called once per frame
    void Update () {
        if(transform.position.y < m_fall)
        {
            m_core.IsDead.Value = true;
        }
    }
}

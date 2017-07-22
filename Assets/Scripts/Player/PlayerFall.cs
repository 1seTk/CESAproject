using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFall : MonoBehaviour
{
    [SerializeField, Range(0, -10), Tooltip("落下許容範囲")]
    private float m_fall = -2.0f;

    private PlayerCore m_core;

	void Start ()
    {
        m_core = GetComponent<PlayerCore>();
    }
	
	
	void Update ()
    {
        // 壁外にいったら
        if (transform.position.x < -4.5f ||
            transform.position.x > 4.5f)
        {
            m_core.IsDead.Value = true;
        }

        // 落ちたら
        if (transform.position.y < m_fall)
        {
            m_core.IsDead.Value = true;
        }
	}
}

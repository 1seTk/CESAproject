using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    // 経過時間
    private float m_elapsedTime = 0.0f;

    void Start ()
    {

    }

    // 更新処理
    void Update ()
    {
        m_elapsedTime += Time.deltaTime;
        Debug.Log(m_elapsedTime);
	}


    /// <summary>
    /// 経過時間をリセット
    /// </summary>
    void Reset()
    {
        m_elapsedTime = 0.0f;
    }

    float GetTime()
    {
        return m_elapsedTime;
    }
}

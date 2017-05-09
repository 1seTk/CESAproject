using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dossunArmy : MonoBehaviour
{
    // 時間
    [SerializeField]
    private float Duration = 0.0f;

    // 待機
    [SerializeField]
    private float Delay = 0.0f;

    // ドッスンのリスト
    private List<GameObject> m_objects = new List<GameObject>();

    // Use this for initialization
    void Awake ()
    {

        // 子のドッスンを取得

        // ドッスンのリセット時間を設定

        // ドッスンの子のスクリプトに時間を設定

        // 待機時間設定

    }

    // Update is called once per frame
    void Update ()
    {
		
	}
}

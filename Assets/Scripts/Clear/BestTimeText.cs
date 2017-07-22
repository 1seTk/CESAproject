using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BestTimeText : MonoBehaviour {

    [SerializeField]
    GameObject m_new;

    static Vector3 m_pos;
    float m_startTime;
    Text m_text;
    bool m_isAplha = false;

    private void Awake()
    {
        RectTransform trans = GetComponent<RectTransform>();
        m_pos = trans.position;
    }


    private void OnEnable()
    {
        RectTransform trans = GetComponent<RectTransform>();
        trans.position = m_pos;

        m_text = GetComponent<Text>();
        string time = Timer.ConvertStringTime(PlayerPrefs.GetFloat(YamagenLib.PlayInstructor.instance.GetLoadStage().ToString() + "Time", 0.0f));
        m_text.text = time;
        trans.DOLocalMoveX(0.0f, 0.1f).SetEase(Ease.Linear)
                        .OnStart(() => Alpha()).SetDelay(1.0f);

        float bestTime = PlayerPrefs.GetFloat(YamagenLib.PlayInstructor.instance.GetLoadStage().ToString() + "Time", float.MaxValue);

        if (bestTime >= Timer.GetTime()) m_new.SetActive(true);
        else m_new.SetActive(false);

            m_isAplha = false;
        float alpha = 0.0f;
        var color = m_text.color;
        color.a = alpha;
        m_text.color = color;
    }

    void Alpha()
    {
        m_startTime = Time.time;
        m_isAplha=true;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_isAplha)
        {
            float timeStep = (Time.time - m_startTime);

            float alpha = timeStep * 0.7f;
            var color = m_text.color;

            if (alpha > 1.0f)
            {
                alpha = 1.0f;
            }
            color.a = alpha;
            m_text.color = color;
        }
    }
}

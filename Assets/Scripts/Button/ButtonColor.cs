using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ButtonColor : MonoBehaviour
{
    //private float textColor;

    private Text m_text;
    private Image m_img;

    private Color m_startTextColor;
    private Color m_startImgColor;

	// Use this for initialization
	void Start ()
    {
        // 色への参照
        m_text = GetComponentInChildren<Text>();
        m_img = GetComponent<Image>();

        // 初期色の取得
        m_startTextColor = m_text.color;
        m_startImgColor = m_img.color;

	}
	
	// Update is called once per frame
	void Update ()
    {

        //if (Input.touchCount > 0)
        //{
        if (Input.GetMouseButtonDown(0))
        {
            DOTween.Kill(this.gameObject);
            //m_text.color = Color.white - m_text.color + Color.black;
            //m_img.color = Color.white - m_img.color + Color.black;
            DOTween.To(
                () => m_text.color,                // 何を対象にするのか
                c => m_text.color = c,    // 値の更新
                Color.white - m_startTextColor + Color.black,                    // 最終的な値
                0.2f                                // アニメーション時間
            );

            DOTween.To(
                    () => m_img.color,                // 何を対象にするのか
                    c => m_img.color = c,    // 値の更新
                    Color.white - m_startImgColor + Color.black,                    // 最終的な値
                    0.2f                                // アニメーション時間
                );
        }
        else if (Input.GetMouseButtonUp(0))
        {
            DOTween.Kill(this.gameObject);
            //m_text.color = Color.white - m_text.color + Color.black;
            //m_img.color = Color.white - m_img.color + Color.black;
            DOTween.To(
                () => m_text.color,                // 何を対象にするのか
                c => m_text.color = c,    // 値の更新
                m_startTextColor,                    // 最終的な値
                0.2f                                // アニメーション時間
            );

            DOTween.To(
                    () => m_img.color,                // 何を対象にするのか
                    c => m_img.color = c,    // 値の更新
                    m_startImgColor,                    // 最終的な値
                    0.2f                                // アニメーション時間
                );

        }
        //Touch touch = Input.GetTouch(0);
        //if (touch.phase == TouchPhase.Began)
        //{                
        //    Debug.Log("color change");
        //    m_text.color = Color.white - m_text.color + Color.black;
        //    m_img.color = Color.white - m_img.color + Color.black;
        //}
        //}
    }


    void ColorTween(Color color)
    {
        DOTween.To(
            () => color,                // 何を対象にするのか
            c => color = c,    // 値の更新
            Color.white - color + Color.black,                    // 最終的な値
            3f                                // アニメーション時間
        );
    }
}

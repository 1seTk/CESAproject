using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(ButtonEx))]
public class ButtonColor : MonoBehaviour
{

    //private float textColor;

    private Text m_text;
    private Image m_img;

    private ButtonEx m_button;

    private Color m_startTextColor;
    private Color m_startImgColor;

    // Use this for initialization
    void Start ()
    {
        // 色への参照
        m_text = GetComponentInChildren<Text>();
        m_img = GetComponent<Image>();

        // ボタンへの参照
        m_button = GetComponent<ButtonEx>();

        // 初期色の取得
        m_startTextColor = m_text.color;
        m_startImgColor = m_img.color;

	}
	
	// Update is called once per frame
	void Update ()
    {
        //if (Input.touchCount > 0)
        //{
        if (m_button.IsOn)
        {
            DOTween.Kill(this.gameObject);
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
        else if (!m_button.IsOn)
        {
            DOTween.Kill(this.gameObject);

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

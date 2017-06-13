using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 山元のやーつ
namespace YamagenLib
{
    public class StageScroll : MonoBehaviour {

        [SerializeField]
        private GameObject content;
        [SerializeField]
        private GameObject buttonPrefab;

        [SerializeField]
        private float margin;

        private int numButtons;

        Texture2D[] texture;

        // Use this for initialization
        void Start()
        {
            texture = SelectManager.instance.GetTexture();

            numButtons = texture.Length;

            float btnHeight = 0;
            for (int i = 0; i < numButtons; i++)
            {
                // 作成
                GameObject button = Instantiate(buttonPrefab) as GameObject;
                RectTransform rectTrans = button.GetComponent<RectTransform>();
                // 作成したオブジェクトを子として登録
                rectTrans.SetParent(content.GetComponent<RectTransform>());
                //テキスト情報を変更
                int stagenum = i + 1;
                button.GetComponentInChildren<Text>().text = "BUTTON" + stagenum.ToString();
                btnHeight = rectTrans.sizeDelta.y;
                rectTrans.localPosition = new Vector2(0, -btnHeight * i - margin * (1 + i));
                // スケールを等倍に
                button.transform.localScale = new Vector3(1, 1, 1);

                button.GetComponent<StageButton>().m_stage = (PlayStage)i;
                button.transform.GetChild(0).GetComponent<Image>().sprite = Sprite.Create(texture[i], new Rect(0.0f, 0.0f, texture[i].width, texture[i].height), new Vector2(0.5f, 0.5f));
            }
            //器であるcontentオブジェクトの高さを、内包するボタンの数に応じて伸長する
            RectTransform contentRectTrans = content.GetComponent<RectTransform>();
            contentRectTrans.sizeDelta = new Vector2(GetComponent<RectTransform>().sizeDelta.x, btnHeight * numButtons + margin * (numButtons + 1));
        }
    }

}
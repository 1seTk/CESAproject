using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowStageName : MonoBehaviour {

    private Text m_txt;
	// Use this for initialization
	void Start () {
        m_txt = GetComponent<Text>();
	}

    void Update()
    {
        m_txt.text = YamagenLib.SelectManager.instance.GetSelectStage().ToString();
    }
}

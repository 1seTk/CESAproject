using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectShowTime : MonoBehaviour {

    private Text m_text;

	// Use this for initialization
	void Start () {
        m_text = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        float bestTime = PlayerPrefs.GetFloat(YamagenLib.SelectManager.instance.GetSelectStage().ToString() + "Time", float.MaxValue);
        string time;
        if (bestTime == float.MaxValue) time = "--：--：--";
        else                            time = Timer.ConvertStringTime(bestTime);
        m_text.text = time;
    }
}

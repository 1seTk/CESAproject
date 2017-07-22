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

    private void Update()
    {
        if (YamagenLib.SceneInstructor.instance.GetLoadScene() == YamagenLib.GameScene.Select)
            m_txt.text = YamagenLib.SelectManager.instance.GetSelectStage().ToString();
        else if (YamagenLib.SceneInstructor.instance.GetLoadScene() == YamagenLib.GameScene.Play)
            if (YamagenLib.PlayInstructor.instance != null) 
            m_txt.text = YamagenLib.PlayInstructor.instance.GetLoadStage().ToString();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBGM : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
        AudioManager.Instance.AudioSet("BGM", "MP3\\bgm");
        AudioManager.Instance.SetOption("BGM", true, false);
        AudioManager.Instance.Play("BGM");
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}

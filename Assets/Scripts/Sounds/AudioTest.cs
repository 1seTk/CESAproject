using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
        AudioManager.Instance.AudioSet("BGM", "MP3\\bgm");
        AudioManager.Instance.Play("BGM");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

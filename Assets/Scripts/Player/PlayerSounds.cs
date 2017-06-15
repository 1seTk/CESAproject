using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{

    private PlayerCore m_core;
    private CheckGround _ground;

	// Use this for initialization
	void Start ()
    {
        AudioManager.Instance.AudioSet("Dead", "MP3\\GrassBroken");
        AudioManager.Instance.AudioSet("ground", "MP3\\tyakuti");

        m_core = GetComponent<PlayerCore>();
        _ground = GetComponent<CheckGround>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        // 地面に接していたら
        if (_ground.IsGround.Value == false)
        {
            AudioManager.Instance.Play("ground");

        }
        // 死んだら
        if (m_core.IsDead.Value == true)
        {
            AudioManager.Instance.Play("Dead");
        }

    }
}

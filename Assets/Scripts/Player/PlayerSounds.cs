using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    [SerializeField]
    private PlayerCore m_core;

	// Use this for initialization
	void Start ()
    {
	}
	
	// Update is called once per frame
	void Update ()
    {
        // 死んだら
        if (m_core.IsDead.Value){
            AudioManager.Instance.Play("Dead");
        }
    }
}

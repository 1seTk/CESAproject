using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowCreate : MonoBehaviour
{
    
public GameObject shadow;

    void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            Instantiate(shadow);
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}

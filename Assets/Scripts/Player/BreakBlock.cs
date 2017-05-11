using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakBlock : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	    if(Input.GetKeyDown(KeyCode.Space))
            Breack();

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.GetInstanceID() == transform.GetInstanceID())
            return;

        Debug.Log("hit");

    }

    void Breack()
    {
        var parts = GetComponentsInChildren<Transform>();

        foreach (var item in parts)
        {
            item.gameObject.AddComponent<Rigidbody>();
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakBlock : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	    //if(Input.GetKeyDown(KeyCode.Space))
     //       Break();

    }

    public void Break()
    {
        var parts = GetComponentsInChildren<Transform>();

		var parent = transform.root;

		transform.parent = null;

        foreach (var item in parts)
        {
            item.gameObject.AddComponent<Rigidbody>();
			item.parent = null;
        }

		Destroy(parent.gameObject, 0.1f);
		Destroy(transform.gameObject, 0.1f);
		// GetComponentInParent<Collider>().enabled = false;
    }
}

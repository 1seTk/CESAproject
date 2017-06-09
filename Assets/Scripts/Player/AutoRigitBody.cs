using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRigitBody : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Rigidbody>() == null && LayerMask.LayerToName(other.gameObject.layer) != "Player" && other.GetComponent<Collider>() == true)
        {
            if (other.gameObject.name == "Floor" || other.gameObject.name == "Wall") return;
            var rb = other.gameObject.AddComponent<Rigidbody>();
            rb.freezeRotation = true;
            rb.useGravity = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Rigidbody>() != null && LayerMask.LayerToName(other.gameObject.layer) != "Player")

            Destroy(other.gameObject.GetComponent<Rigidbody>());
    }
}

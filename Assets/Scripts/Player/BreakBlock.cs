using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakBlock : MonoBehaviour {

	[SerializeField, Tooltip("死んだときに破裂するか")]
	private bool m_useBomb = true;

    public void Break()
    {
        var parts = GetComponentsInChildren<Transform>();

		var parent = transform.parent;

		transform.parent = null;

        foreach (var item in parts)
        {
			if(m_useBomb)
				item.gameObject.layer = LayerMask.NameToLayer("Default");
            item.gameObject.AddComponent<Rigidbody>();
			item.parent = null;
        }

		Destroy(parent.gameObject, 0.1f);
		Destroy(transform.gameObject, 0.1f);
		// GetComponentInParent<Collider>().enabled = false;
    }
}

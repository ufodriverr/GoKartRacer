using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TNT_Item : cls_Item {

	public override void UseItem () {
		Rigidbody temp = Instantiate (Global.Instance.TntItem,Global.Instance.ItemsParent);
		temp.transform.position = KartRef.transform.position - KartRef.transform.forward * 1.25f;
		temp.transform.rotation = KartRef.transform.rotation;
		temp.velocity = Vector3.up * 30;
	}

	void OnTriggerEnter (Collider coll) {
		KartReferences temp = coll.GetComponentInParent<KartReferences>();
		if (temp != null) {
			if (!temp.KartPhysics.KnockOut) {
				temp.KartPhysics.KnockOut = true;
				Destroy (gameObject);
			}
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TNT_Item : cls_Item {

	public override void UseItem () {
		Rigidbody temp = Instantiate (Global.Instance.TntItem,Global.Instance.ItemsParent);
		temp.transform.position = KartRef.transform.position - KartRef.transform.forward * 1.25f + Vector3.up*0.5f;
		temp.transform.rotation = KartRef.transform.rotation;
		temp.velocity = Vector3.up * 7;
		KartRef = null;
	}

	void OnCollisionEnter (Collision coll) {
		if (KartRef == null) {
			KartReferences temp = coll.gameObject.GetComponentInParent<KartReferences> ();
			if (temp != null) {
				if (!temp.KartPhysics.KnockOut) {
					temp.KartPhysics.KnockOut = true;
					Destroy (gameObject);
				}
			}
		}
	}
}

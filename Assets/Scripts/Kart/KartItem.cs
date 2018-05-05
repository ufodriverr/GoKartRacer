using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(KartReferences))]
public class KartItem : MonoBehaviour {

	KartReferences KartRef;

	public cls_Item HoldingItem;

	void Awake(){
		KartRef = GetComponent<KartReferences> ();
		KartRef.KartItem = this;
	}

	public void UseItem(){
		if (HoldingItem) {
			HoldingItem.KartRef = KartRef;
			HoldingItem.UseItem ();
			Destroy (HoldingItem);
			HoldingItem = null;
		}
	}
}

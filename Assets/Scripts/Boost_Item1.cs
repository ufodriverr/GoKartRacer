using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boost_Item1 : cls_Item {

	public float LongTime;
	public float BoostPercent;
	public float InitAcceleration;

	public override void UseItem () {
		int id = KartRef.KartEffects.FindEffect (cls_Effect.Effect_Type.Boost);
		if (id != -1) {
			KartRef.KartEffects.AllEffects [id].LongTime = LongTime;
			KartRef.KartEffects.AllEffects [id].ttimer = 0;
			KartRef.KartEffects.AllEffects [id].BoostPercent = BoostPercent;
			KartRef.KartEffects.AllEffects [id].InitAcceleration = InitAcceleration;
		} else {
			Effect_Boost tempB = KartRef.gameObject.AddComponent<Effect_Boost> ();
			tempB.BoostPercent = BoostPercent;
			tempB.LongTime = LongTime;
			tempB.InitAcceleration = InitAcceleration;
		}
	}

	void OnTriggerEnter (Collider coll) {
		KartReferences temp = coll.GetComponentInParent<KartReferences>();
		if (temp != null) {
			if (temp.KartItem.HoldingItem == null) {
				Boost_Item1 Ttemp = temp.gameObject.AddComponent<Boost_Item1> ();
				Ttemp.BoostPercent = BoostPercent;
				Ttemp.InitAcceleration = InitAcceleration;
				Ttemp.LongTime = LongTime;
				temp.KartItem.HoldingItem = Ttemp;
				Destroy (gameObject);
			}
		}
	}
}

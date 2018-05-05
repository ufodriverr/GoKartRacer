using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostPanel : MonoBehaviour {

	List<KartReferences> CarsInRange = new List<KartReferences>();

	public float BoostPowerPercent;
	public float BoostInitialPower;
	public float BoostTime;

	void Update(){
		for (int x = 0; x < CarsInRange.Count; x++) {
			int id = CarsInRange[x].KartEffects.FindEffect (cls_Effect.Effect_Type.Boost);
			if (id != -1) {
				CarsInRange[x].KartEffects.AllEffects [id].LongTime = BoostTime;
				CarsInRange[x].KartEffects.AllEffects [id].ttimer = 0;
				CarsInRange[x].KartEffects.AllEffects [id].BoostPercent = BoostPowerPercent;
				CarsInRange[x].KartEffects.AllEffects [id].InitAcceleration = BoostInitialPower;
			} else {
				Effect_Boost tempEff = CarsInRange[x].gameObject.AddComponent<Effect_Boost> ();
				tempEff.LongTime = BoostTime;
				tempEff.BoostPercent = BoostPowerPercent;
				tempEff.InitAcceleration = BoostInitialPower;
			}
		}
	}

	void OnTriggerEnter (Collider coll) {
		KartReferences temp = coll.GetComponentInParent<KartReferences>();
		if (temp != null) {
			CarsInRange.Add (temp);
		}
	}

	void OnTriggerExit (Collider coll) {
		KartReferences temp = coll.GetComponentInParent<KartReferences>();
		if (temp != null) {
			CarsInRange.Remove (temp);
		}
	}
}

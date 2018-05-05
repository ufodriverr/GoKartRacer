using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(KartReferences))]
public class KartEffects : MonoBehaviour {

	[HideInInspector]
	public KartReferences KartRef;

	public List<cls_Effect> AllEffects;

	void Awake(){
		AllEffects = new List<cls_Effect> ();
		KartRef = GetComponent<KartReferences> ();
		KartRef.KartEffects = this;
	}

	public int FindEffect(cls_Effect.Effect_Type Type){
		for (int i = 0; i < AllEffects.Count; i++) {
			if (AllEffects [i].Type == Type) {
				return i;
			}
		}
		return -1;
	}
}

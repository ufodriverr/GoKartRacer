using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(KartEffects))]
public class Effect_LostControll : cls_Effect {

	KartEffects KartEffects;

	void Awake(){
		Type = Effect_Type.LostControll;
		KartEffects = GetComponent<KartEffects> ();
		KartEffects.AllEffects.Add (this);
	}

	void FixedUpdate(){
		//KartEffects.KartRef.Animator
		KartEffects.KartRef.KartVisual.transform.localEulerAngles=Vector3.up*ttimer*360*2;
		ttimer += Time.fixedDeltaTime;
		if (ttimer >= LongTime) {
			KartEffects.KartRef.KartPhysics.LostControll = false;
			KartEffects.AllEffects.Remove (this);
			KartEffects.KartRef.Animator.transform.localEulerAngles = Vector3.zero;
			Destroy (this);
		}
	}

}

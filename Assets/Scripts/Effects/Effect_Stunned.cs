using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(KartEffects))]
public class Effect_Stunned : cls_Effect {

	KartEffects KartEffects;

	void Awake(){
		Type = Effect_Type.Knockout;
		KartEffects = GetComponent<KartEffects> ();
		KartEffects.AllEffects.Add (this);
	}

	void FixedUpdate(){
		//KartEffects.KartRef.Animator
		KartEffects.KartRef.KartVisual.transform.localEulerAngles=Vector3.up*ttimer*360*2+Vector3.forward*ttimer*360;
		KartEffects.KartRef.KartVisual.transform.localPosition = Vector3.up * (1+Mathf.Sin (ttimer*20))/3f;
		ttimer += Time.fixedDeltaTime;
		if (ttimer >= LongTime) {
			KartEffects.KartRef.KartVisual.transform.localPosition = Vector3.zero;
			KartEffects.KartRef.KartPhysics.LostControll = false;
			KartEffects.AllEffects.Remove (this);
			KartEffects.KartRef.Animator.transform.localEulerAngles = Vector3.zero;
			Destroy (this);
		}
	}
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(KartEffects))]
public class Effect_Boost : cls_Effect {

	KartEffects KartEffects;

	void Awake(){
		Type = Effect_Type.Boost;
		KartEffects = GetComponent<KartEffects> ();
		KartEffects.AllEffects.Add (this);
	}

	void FixedUpdate(){
		KartEffects.KartRef.KartParams.CurMaxSpeed += Mathf.Clamp(KartEffects.KartRef.KartParams.MaxSpeed * BoostPercent-KartEffects.KartRef.KartParams.MaxSpeed,0,9999);
		if (InitAcceleration != 0) {
			KartEffects.KartRef.KartParams.CurAcceleration = InitAcceleration;
			InitAcceleration = 0;
		} else {
			KartEffects.KartRef.KartParams.CurAcceleration += Mathf.Clamp(KartEffects.KartRef.KartParams.Acceleration * BoostPercent-KartEffects.KartRef.KartParams.Acceleration,0,9999);
		}
		ttimer += Time.fixedDeltaTime;
		if (ttimer >= LongTime) {
			KartEffects.AllEffects.Remove (this);
			Destroy (this);
		}
	}

}

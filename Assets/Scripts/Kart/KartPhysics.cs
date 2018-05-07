using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(KartReferences))]
public class KartPhysics : MonoBehaviour {

	[HideInInspector]
	public float ZAxis;
	[HideInInspector]
	public float RotateAxis;
	[HideInInspector]
	public bool Jump;
	[HideInInspector]
	public bool Grounded = true;
	[HideInInspector]
	public bool LostControll;
	[HideInInspector]
	public bool KnockOut;
	[HideInInspector]
	public int BoostedTimes = 0;

	[HideInInspector]
	public bool Drift = false;
	[HideInInspector]
	public bool DriftBoost = false;
	[HideInInspector]
	public float driftTimer = 0;

	[HideInInspector]
	public Vector3 curVel;
	[HideInInspector]
	public Vector3 curVelLocal;
	[HideInInspector]
	public Vector3 curAngularVel;

	public float SBoostPercent;
	public float SBoostPercentT;
	public float SBoostPercent2;
	public float SBoostPercent2T;
	public float BoostPercent;
	public float BoostPercentT;

	KartReferences KartRef;
	void Awake(){
		KartRef = GetComponent<KartReferences> ();
		KartRef.KartPhysics = this;
	}


	void Update(){
		//Check for ground
		if (Physics.Raycast (transform.position, Vector3.down, 0.3f)) {
			if (Grounded == false) {
				KartRef.Animator.Play ("Land");
			}
			Grounded = true;
		} else {
			Grounded = false;
		}
		//Enable,Disable tails
		if (Drift && Grounded) {
			EnableTrails (true);
			driftTimer += Time.deltaTime;
			float BoostVal = Mathf.Clamp01(driftTimer / KartRef.KartParams.MaxBoostTimer);
			KartRef.BoostVisual.fillAmount = BoostVal;
			if (driftTimer > KartRef.KartParams.DriftLoseControl) {
				LostControll = true;
			}
		} else {
			if (!LostControll) {
				EnableTrails (false);
			}
			driftTimer = 0;
			if (KartRef.BoostVisual) {
				KartRef.BoostVisual.fillAmount = 0.01f;
			}
			BoostedTimes = 0;
		}

		float TargetPitch = 0.6f + Mathf.Clamp (curVelLocal.z / KartRef.KartParams.MaxSpeed, 0, 1.25f) * 1.5f;
		KartRef.KartEngineSFX.pitch = Mathf.Lerp(KartRef.KartEngineSFX.pitch,TargetPitch,Time.deltaTime*10f);
		//Rotate KartRef.Wheels
		KartRef.Wheels[0].Rotate(Vector3.up*curVelLocal.z*KartRef.KartParams.FWheelsRotSpeed*Time.deltaTime,Space.Self);
		KartRef.Wheels[1].Rotate(Vector3.up*curVelLocal.z*KartRef.KartParams.FWheelsRotSpeed*Time.deltaTime,Space.Self);
		KartRef.Wheels[2].Rotate(Vector3.up*curVelLocal.z*KartRef.KartParams.BWheelsRotSpeed*Time.deltaTime,Space.Self);
		KartRef.Wheels[3].Rotate(Vector3.up*curVelLocal.z*KartRef.KartParams.BWheelsRotSpeed*Time.deltaTime,Space.Self);

		KartRef.Wheels [0].transform.parent.transform.localRotation = Quaternion.Lerp (KartRef.Wheels [0].transform.parent.transform.localRotation,Quaternion.Euler(Vector3.up*30*RotateAxis),Time.deltaTime*10);
		KartRef.Wheels [1].transform.parent.transform.localRotation = Quaternion.Lerp (KartRef.Wheels [1].transform.parent.transform.localRotation,Quaternion.Euler(Vector3.up*30*RotateAxis),Time.deltaTime*10);
	}

	void FixedUpdate(){
		curVel = KartRef.RigidBody.velocity;
		curAngularVel = KartRef.RigidBody.angularVelocity;
		curVelLocal = KartRef.KartVisual.InverseTransformPoint(KartRef.KartVisual.transform.position+KartRef.RigidBody.velocity);

		if (LostControll) {
			Drift = false;
			//Add Lost Controll Effect
			int id = KartRef.KartEffects.FindEffect(cls_Effect.Effect_Type.LostControll);
			if (id == -1){
				Effect_LostControll tempEff = gameObject.AddComponent<Effect_LostControll>();
				tempEff.LongTime = 1;
			}
		} else if(KnockOut){
			Drift = false;
			//Add KnockOut Effect
			int id = KartRef.KartEffects.FindEffect(cls_Effect.Effect_Type.Knockout);
			if (id == -1){
				Effect_Stunned tempEff = gameObject.AddComponent<Effect_Stunned>();
				tempEff.LongTime = 2;
			}
		} else{
			//Forward : Backward
			float desiredZvel = curVelLocal.z + KartRef.KartParams.CurAcceleration * ZAxis;
			float desiredZvelABS = Mathf.Abs (desiredZvel);
			if (ZAxis > 0) {
				if (desiredZvelABS <= KartRef.KartParams.CurMaxSpeed) {
					curVelLocal.z = desiredZvel;
				} else {
					float overSpeed;
					overSpeed = desiredZvelABS - KartRef.KartParams.CurMaxSpeed;
					float lastAcc = desiredZvelABS - (overSpeed + Mathf.Abs (curVelLocal.z));
					curVelLocal.z += Mathf.Clamp (lastAcc, 0f, 999f) * ZAxis;
				}
			} else {
				if (desiredZvelABS <= KartRef.KartParams.MaxBackSpeed) {
					curVelLocal.z = desiredZvel;
				} else {
					float overSpeed;
					overSpeed = desiredZvelABS - KartRef.KartParams.MaxBackSpeed;
					float lastAcc = desiredZvelABS - (overSpeed + Mathf.Abs (curVelLocal.z));
					curVelLocal.z += Mathf.Clamp (lastAcc, 0f, 999f) * ZAxis;
				}
			}
			//Rotation
			float zsign = Mathf.Sign (curVelLocal.z);

			if (Mathf.Abs (curVelLocal.z) > 0.2f) {
				curAngularVel.y = KartRef.KartParams.CurTurnSpeed * RotateAxis * zsign * Mathf.Clamp (Mathf.Abs (curVelLocal.z / (30f / 6)), 0.6f, 1f);
			} else {
				curAngularVel.y = 0;
			}
			//Jump
			if (Jump && Grounded) {
				if (RotateAxis != 0) {
					Drift = true;
				}
				curVelLocal.y += KartRef.KartParams.JumpPower;
				Jump = false;
			}
		}
		//Apply drag
		if (Mathf.Abs (curVelLocal.x) >= KartRef.KartParams.SideDrag) {
			if (Drift) {
				curVelLocal.x -= Mathf.Sign (curVelLocal.x) * KartRef.KartParams.DriftDrag;
			} else {
				curVelLocal.x -= Mathf.Sign (curVelLocal.x) * KartRef.KartParams.SideDrag;
			}
		} else {
			curVelLocal.x = 0;
		}
		if (Mathf.Abs (curVelLocal.z) >= KartRef.KartParams.FrontDrag) {
			curVelLocal.z -= Mathf.Sign (curVelLocal.z) * KartRef.KartParams.FrontDrag;
		} else {
			curVelLocal.z = 0;
		}

		//Apply boost
		if (DriftBoost) {
			DriftBoost = false;
			float BoostVal = Mathf.Clamp01(driftTimer / KartRef.KartParams.MaxBoostTimer);
			if (BoostVal > 0.9f) {
				BoostedTimes++;
			} else {
				BoostedTimes = 0;
			}
			if (BoostedTimes >= 3) {
				//curVelLocal.z +=KartRef.KartParams.BoostPower * 2;
				int id = KartRef.KartEffects.FindEffect(cls_Effect.Effect_Type.Boost);
				if (id != -1) {
					KartRef.KartEffects.AllEffects[id].LongTime = BoostPercentT;
					KartRef.KartEffects.AllEffects [id].ttimer = 0;
					KartRef.KartEffects.AllEffects [id].BoostPercent = BoostPercent* BoostVal;
					KartRef.KartEffects.AllEffects[id].InitAcceleration = KartRef.KartParams.BoostPower * 2;
				} else {
					Effect_Boost tempEff = gameObject.AddComponent<Effect_Boost>();
					tempEff.LongTime = BoostPercentT;
					tempEff.BoostPercent = BoostPercent* BoostVal;
					tempEff.InitAcceleration = KartRef.KartParams.BoostPower * 2;
				}
				Drift = false;
			}else if (BoostedTimes == 2) {
				int id = KartRef.KartEffects.FindEffect(cls_Effect.Effect_Type.Boost);
				if (id != -1) {
					KartRef.KartEffects.AllEffects[id].LongTime = SBoostPercent2T;
					KartRef.KartEffects.AllEffects [id].ttimer = 0;
					KartRef.KartEffects.AllEffects [id].BoostPercent = SBoostPercent2* BoostVal;
					KartRef.KartEffects.AllEffects[id].InitAcceleration = KartRef.KartParams.BoostPower * BoostVal;
				} else {
					Effect_Boost tempEff = gameObject.AddComponent<Effect_Boost>();
					tempEff.LongTime = SBoostPercent2T;
					tempEff.BoostPercent = SBoostPercent2* BoostVal;
					tempEff.InitAcceleration = KartRef.KartParams.BoostPower * BoostVal;
				}
			} else {
				//curVelLocal.z += KartRef.KartParams.BoostPower * BoostVal;
				int id = KartRef.KartEffects.FindEffect(cls_Effect.Effect_Type.Boost);
				if (id != -1) {
					KartRef.KartEffects.AllEffects[id].LongTime = SBoostPercentT;
					KartRef.KartEffects.AllEffects [id].ttimer = 0;
					KartRef.KartEffects.AllEffects [id].BoostPercent = SBoostPercent* BoostVal;
					KartRef.KartEffects.AllEffects[id].InitAcceleration = KartRef.KartParams.BoostPower * BoostVal;
				} else {
					Effect_Boost tempEff = gameObject.AddComponent<Effect_Boost>();
					tempEff.LongTime = SBoostPercentT;
					tempEff.BoostPercent = SBoostPercent* BoostVal;
					tempEff.InitAcceleration = KartRef.KartParams.BoostPower * BoostVal;
				}
			}
			driftTimer = 0;
		}

		curAngularVel.x = 0;
		curAngularVel.z = 0;

		KartRef.RigidBody.velocity = KartRef.KartVisual.TransformPoint(curVelLocal)-KartRef.KartVisual.transform.position;
		KartRef.RigidBody.angularVelocity = curAngularVel;

		//Stop kart from rotating around x and z axis
		if (!Grounded) {
			Vector3 curLocalEuler = transform.localEulerAngles;
			curLocalEuler.x = 0;
			curLocalEuler.z = 0;
			transform.rotation = Quaternion.Lerp (transform.localRotation, Quaternion.Euler (curLocalEuler), 0.1f);
		}

		KartRef.KartParams.ResetParams ();
	}

	public void EnableTrails(bool value){
		float tiresVolume = 0.345f*Global.Instance.SFXVolume;
		float SpeedProcess = 10;
		if (value) {
			KartRef.KartTireDriftSFX.volume = Mathf.Lerp (KartRef.KartTireDriftSFX.volume,tiresVolume,Time.deltaTime*SpeedProcess);
		} else {
			KartRef.KartTireDriftSFX.volume = Mathf.Lerp (KartRef.KartTireDriftSFX.volume,0,Time.deltaTime*SpeedProcess);
		}
		for (int i = 0; i < KartRef.Trail.Length; i++) {
			KartRef.Trail [i].emitting = value;
		}
	}

}

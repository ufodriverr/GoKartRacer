using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(KartReferences))]
public class KartPlayer : MonoBehaviour {

	KartReferences KartRef;
	void Awake(){
		KartRef = GetComponent<KartReferences> ();
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.T)) {
			
		}

		//Reset player if falloff
		if (transform.position.y < -1f || Input.GetKeyDown(KeyCode.R)) {
			transform.position = KartRef.PlayerSpawn.position;
			transform.rotation = KartRef.PlayerSpawn.rotation;
			KartRef.RigidBody.velocity = Vector3.zero;
		}

		//Convert.ToInt16 ();
		KartRef.KartPhysics.ZAxis = Convert.ToInt16 (Input.GetKey(KartRef.KartInputs.ForwardButton)) - Convert.ToInt16 (Input.GetKey(KartRef.KartInputs.BackButton));
		//KartRef.KartPhysics.Brake = Input.GetKey (KartRef.KartInputs.BrakeButton);
		KartRef.KartPhysics.RotateAxis = Convert.ToInt16 (Input.GetKey(KartRef.KartInputs.TurnRightButton)) - Convert.ToInt16 (Input.GetKey(KartRef.KartInputs.TurnLeftButton));
		KartRef.SpeedText.text = Mathf.Round (KartRef.KartPhysics.curVelLocal.magnitude*6f).ToString()+" km/h";

		//Apply Jump in order to use it in FixedUpdate
		if(Input.GetKeyDown (KartRef.KartInputs.JumpButton)){
			KartRef.Animator.Play ("Jump");
			KartRef.KartPhysics.Jump = true;
		}

		if (Input.GetKeyDown (KartRef.KartInputs.BoostButton) && KartRef.KartPhysics.Drift) {
			KartRef.KartPhysics.DriftBoost = true;
		}

		//Apply Drift in order to use it in FixedUpdate
		if (!Input.GetKey (KartRef.KartInputs.JumpButton) || KartRef.KartPhysics.curVelLocal.magnitude<KartRef.KartParams.DriftMinSpeed || (Mathf.Abs(KartRef.KartPhysics.curVelLocal.x)<KartRef.KartParams.DriftMinSideSpeed) && KartRef.KartPhysics.curVelLocal.y<=0) {
			KartRef.KartPhysics.Drift = false;
		}

		//Use Item
		if (Input.GetKeyDown (KartRef.KartInputs.UseItemButton)) {
			KartRef.KartItem.UseItem ();			
		}
	}
}

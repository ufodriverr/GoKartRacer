using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(KartReferences))]
public class KartParams : MonoBehaviour {

	[Header("Params")]
	public float MaxSpeed = 20f;
	public float MaxBackSpeed = 2.5f;
	public float Acceleration = 0.3f;
	public float TurnSpeed = 2f;
	public float JumpPower = 3f;
	public float BoostPower = 10f;
	public float MaxBoostTimer = 0.33f;

	[Header("Drag params")]
	public float SideDrag = 0.5f;
	public float DriftDrag = 0.2f;
	public float FrontDrag = 0.0001f;
	public float DriftLoseControl = 2;
	public float DriftMinSpeed = 8;
	public float DriftMinSideSpeed = 3;

	[Header("Wheels rot Visual")]
	public float FWheelsRotSpeed;
	public float BWheelsRotSpeed;

	[HideInInspector]
	public float CurMaxSpeed;
	[HideInInspector]
	public float CurAcceleration;
	[HideInInspector]
	public float CurTurnSpeed;
	[HideInInspector]
	public float CurBoostPower;
	[HideInInspector]
	public float CurSideDrag;
	[HideInInspector]
	public float CurDriftDrag;

	KartReferences KartRef;

	void Awake(){
		KartRef = GetComponent<KartReferences> ();
		KartRef.KartParams = this;

		CurMaxSpeed = MaxSpeed;
		CurAcceleration = Acceleration;
		CurTurnSpeed = TurnSpeed;
		CurBoostPower = BoostPower;
		CurSideDrag = SideDrag;
		CurDriftDrag = DriftDrag;
	}

	public void ResetParams(){
		CurMaxSpeed = MaxSpeed;
		CurAcceleration = Acceleration;
		CurTurnSpeed = TurnSpeed;
		CurBoostPower = BoostPower;
		CurSideDrag = SideDrag;
		CurDriftDrag = DriftDrag;
	}
}

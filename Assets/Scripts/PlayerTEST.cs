using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PlayerTEST : MonoBehaviour {

	[Header("References")]
	public Rigidbody RigidBody;
	public Animator Animator;
	public Transform[] Wheels;
	public Transform KartVisual;
	public TrailRenderer[] Trail;
	public Text SpeedText;
	public Image BoostVisual;
	public AnimationClip LostControlAnimation;
	public Transform PlayerSpawn;

	[Header("Wheels rot Visual")]
	public float FWheelsRotSpeed;
	public float BWheelsRotSpeed;

	[Header("Params")]
	public float MaxSpeed;
	public float MaxBackSpeed;
	public float Acceleration;
	public float TurnSpeed;
	public float JumpPower;
	public float BoostPower;
	public float MaxBoostTimer=1;

	[Header("Drag params")]
	public float SideDrag = 1;
	public float DriftDrag = 1;
	public float FrontDrag = 1;
	public float DriftLoseControl = 3;
	public float DriftMinSpeed = 3;
	public float DriftMinSideSpeed = 3;

	[Header("Input")]
	public KeyCode ForwardButton = KeyCode.W;
	public KeyCode BackButton = KeyCode.S;
	public KeyCode TurnRightButton = KeyCode.D;
	public KeyCode TurnLeftButton = KeyCode.A;
	public KeyCode BrakeButton = KeyCode.LeftControl;
	public KeyCode JumpButton = KeyCode.Space;
	public KeyCode BoostButton = KeyCode.LeftShift;

	[Header("Visual")]
	public float visualSpeedMult = 6f;

	float ZAxis;
	float RotateAxis;
	bool Brake;
	bool Jump;
	bool Grounded = true;
	bool LostControll;
	float LostControllTimer = 0;
	int BoostedTimes = 0;

	bool Drift = false;
	bool DriftBoost = false;
	float driftTimer = 0;

	Vector3 curVel;
	Vector3 curVelLocal;
	Vector3 curAngularVel;

	void Start () {
		EnableTrails (false);
	}
	
	void Update () {
		//Reset player if falloff
		if (transform.position.y < -1f || Input.GetKeyDown(KeyCode.R)) {
			transform.position = PlayerSpawn.position;
			transform.rotation = PlayerSpawn.rotation;
			RigidBody.velocity = Vector3.zero;
		}

		//Convert.ToInt16 ();
		ZAxis = Convert.ToInt16 (Input.GetKey(ForwardButton)) - Convert.ToInt16 (Input.GetKey(BackButton));
		Brake = Input.GetKey (BrakeButton);
		RotateAxis = Convert.ToInt16 (Input.GetKey(TurnRightButton)) - Convert.ToInt16 (Input.GetKey(TurnLeftButton));
		SpeedText.text = Mathf.Round (curVelLocal.magnitude*visualSpeedMult).ToString()+" km/h";

		//Check for ground
		if (Physics.Raycast (transform.position, Vector3.down, 0.3f)) {
			if (Grounded == false) {
				Animator.Play ("Land");
			}
			Grounded = true;
		} else {
			Grounded = false;
		}
		//Apply Jump in order to use it in FixedUpdate
		if(Input.GetKeyDown (JumpButton)){
			Animator.Play ("Jump");
			Jump = true;
		}

		if (Input.GetKeyDown (BoostButton) && Drift) {
			DriftBoost = true;
		}

		//Apply Drift in order to use it in FixedUpdate
		if (!Input.GetKey (JumpButton) || curVelLocal.magnitude<DriftMinSpeed || (Mathf.Abs(curVelLocal.x)<DriftMinSideSpeed) && curVelLocal.y<=0) {
			Drift = false;
		}
		//Enable,Disable tails
		if (Drift && Grounded) {
			EnableTrails (true);
			driftTimer += Time.deltaTime;
			float BoostVal = Mathf.Clamp01(driftTimer / MaxBoostTimer);
			BoostVisual.fillAmount = BoostVal;
			if (driftTimer > DriftLoseControl) {
				LostControll = true;
			}
		} else {
			if (!LostControll) {
				EnableTrails (false);
			}
			driftTimer = 0;
			BoostVisual.fillAmount = 0.01f;
			BoostedTimes = 0;
		}
		//Rotate Wheels
		Wheels[0].Rotate(Vector3.up*curVelLocal.z*FWheelsRotSpeed*Time.deltaTime,Space.Self);
		Wheels[1].Rotate(Vector3.up*curVelLocal.z*FWheelsRotSpeed*Time.deltaTime,Space.Self);
		Wheels[2].Rotate(Vector3.up*curVelLocal.z*BWheelsRotSpeed*Time.deltaTime,Space.Self);
		Wheels[3].Rotate(Vector3.up*curVelLocal.z*BWheelsRotSpeed*Time.deltaTime,Space.Self);

		Wheels [0].transform.parent.transform.localRotation = Quaternion.Lerp (Wheels [0].transform.parent.transform.localRotation,Quaternion.Euler(Vector3.up*30*RotateAxis),Time.deltaTime*10);
		Wheels [1].transform.parent.transform.localRotation = Quaternion.Lerp (Wheels [1].transform.parent.transform.localRotation,Quaternion.Euler(Vector3.up*30*RotateAxis),Time.deltaTime*10);
	}

	void FixedUpdate(){
		curVel = RigidBody.velocity;
		curAngularVel = RigidBody.angularVelocity;
		curVelLocal = KartVisual.InverseTransformPoint(KartVisual.transform.position+RigidBody.velocity);

		if (LostControll) {
			if (LostControllTimer == 0) {
				Animator.Play ("LostControll");
			}
			Drift = false;
			LostControllTimer += Time.fixedDeltaTime;
			if (LostControllTimer >= LostControlAnimation.length) {
				LostControllTimer = 0;
				LostControll = false;
			}
		} else {
			LostControllTimer = 0;
			//Forward : Backward
			float desiredZvel = curVelLocal.z + Acceleration * ZAxis;
			float desiredZvelABS = Mathf.Abs (desiredZvel);
			if (curVelLocal.z > 0) {
				if (desiredZvelABS <= MaxSpeed) {
					curVelLocal.z = desiredZvel;
				} else {
					float overSpeed;
					overSpeed = desiredZvelABS - MaxSpeed;
					float lastAcc = desiredZvelABS - (overSpeed + Mathf.Abs (curVelLocal.z));
					curVelLocal.z += Mathf.Clamp (lastAcc, 0f, 999f) * ZAxis;
				}
			} else {
				if (desiredZvelABS <= MaxBackSpeed) {
					curVelLocal.z = desiredZvel;
				} else {
					float overSpeed;
					overSpeed = desiredZvelABS - MaxBackSpeed;
					float lastAcc = desiredZvelABS - (overSpeed + Mathf.Abs (curVelLocal.z));
					curVelLocal.z += Mathf.Clamp (lastAcc, 0f, 999f) * ZAxis;
				}
			}
			//Rotation
			float zsign = Mathf.Sign (curVelLocal.z);

			if (Mathf.Abs (curVelLocal.z) > 0.2f) {
				curAngularVel.y = TurnSpeed * RotateAxis * zsign * Mathf.Clamp (Mathf.Abs (curVelLocal.z / (30f / visualSpeedMult)), 0.6f, 1f);
			}
			//Jump
			if (Jump && Grounded) {
				if (RotateAxis != 0) {
					Drift = true;
				}
				curVelLocal.y += JumpPower;
				Jump = false;
			}
		}
		//Apply drag
		if (Mathf.Abs (curVelLocal.x) >= SideDrag) {
			if (Drift) {
				curVelLocal.x -= Mathf.Sign (curVelLocal.x) * DriftDrag;
			} else {
				curVelLocal.x -= Mathf.Sign (curVelLocal.x) * SideDrag;
			}
		} else {
			curVelLocal.x = 0;
		}
		if (Mathf.Abs (curVelLocal.z) >= FrontDrag) {
			curVelLocal.z -= Mathf.Sign (curVelLocal.z) * FrontDrag;
		} else {
			curVelLocal.z = 0;
		}

		//Apply boost
		if (DriftBoost) {
			DriftBoost = false;
			float BoostVal = Mathf.Clamp01(driftTimer / MaxBoostTimer);
			if (BoostVal > 0.9f) {
				BoostedTimes++;
			} else {
				BoostedTimes = 0;
			}
			if (BoostedTimes >= 3) {
				curVelLocal.z += BoostPower * 2;
				Drift = false;
			} else {
				curVelLocal.z += BoostPower * BoostVal;
			}
			driftTimer = 0;
		}

		curAngularVel.x = 0;
		curAngularVel.z = 0;

		RigidBody.velocity = KartVisual.TransformPoint(curVelLocal)-KartVisual.transform.position;
		RigidBody.angularVelocity = curAngularVel;

		//Stop kart from rotating around x and z axis
		if (!Grounded) {
			Vector3 curLocalEuler = transform.localEulerAngles;
			curLocalEuler.x = 0;
			curLocalEuler.z = 0;
			transform.rotation = Quaternion.Lerp (transform.localRotation, Quaternion.Euler (curLocalEuler), 0.1f);
		}
	}

	void EnableTrails(bool value){
		for (int i = 0; i < Trail.Length; i++) {
			Trail [i].emitting = value;
		}
	}
}

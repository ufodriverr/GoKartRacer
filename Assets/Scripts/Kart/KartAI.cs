using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(KartReferences))]
public class KartAI : MonoBehaviour {

	public Transform[] Nodes;
	int CurNode = 0;
	bool SetTarget = false;
	KartReferences KartRef;
	Vector3 transformedToLocal;
	Vector3 TargetWorld;

	float UPDTtimer = 0;
	Vector3 PrevPos;

	void Awake(){
		KartRef = GetComponent<KartReferences> ();
		PrevPos = transform.position;
	}

	void Update () {
		UPDTtimer += Time.deltaTime;
		if (UPDTtimer > 1f) {
			UPDTtimer = 0;
			if ((transform.position - PrevPos).sqrMagnitude < 0.5f) {
				ResetPos ();
			} else {
				PrevPos = transform.position;
			}
		}
		if (transform.position.y < -1f) {
			ResetPos ();
		}
		if (!SetTarget) {
			TargetWorld = Nodes [CurNode].position + Nodes [CurNode].right * Random.Range (-0.5f, 0.5f);
			SetTarget = true;
		}
		transformedToLocal = transform.InverseTransformPoint (TargetWorld);
		if (transformedToLocal.z < 0.1f) {
			SetTarget = false;
			CurNode++;
			if (CurNode >= Nodes.Length) {
				CurNode = 0;
			}
		}
		Vector3 curVelLocal = KartRef.KartVisual.InverseTransformPoint(KartRef.KartVisual.transform.position+KartRef.RigidBody.velocity);
		if (curVelLocal.z > (120f/6f)) {
			KartRef.KartPhysics.ZAxis = 0;
		} else {
			KartRef.KartPhysics.ZAxis = 1;
		}
		float CalculatedAngle = Vector3.Angle (transform.forward, (TargetWorld - transform.position));
		if (CalculatedAngle > 10f) {
			KartRef.KartPhysics.RotateAxis = Mathf.Sign (transformedToLocal.x);
		} else {
			KartRef.KartPhysics.RotateAxis = (CalculatedAngle / 10);
		}

	}

	void ResetPos(){
		transform.position = KartRef.PlayerSpawn.position;
		transform.rotation = KartRef.PlayerSpawn.rotation;
		KartRef.RigidBody.velocity = Vector3.zero;
		CurNode = 0;
		SetTarget = false;
	}
}

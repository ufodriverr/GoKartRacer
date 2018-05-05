using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

	public Transform TargetPos;
	public Transform LookAt;
	public float LerpSpeed=10;

	void FixedUpdate () {
		transform.position = Vector3.Lerp (transform.position,TargetPos.position,LerpSpeed);
		transform.LookAt (LookAt.position);
		/*Quaternion Rot = transform.rotation;
		transform.LookAt (LookAt.position);
		Quaternion RemRot = transform.rotation;
		transform.rotation = Rot;
		transform.rotation = Quaternion.Lerp (transform.rotation,RemRot,LerpSpeed);*/

	}
}

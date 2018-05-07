using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatAnimation : MonoBehaviour {

	public AnimationCurve XCurve;
	public AnimationCurve YCurve;
	public AnimationCurve ZCurve;

	public float ValuesMult = 0.3f;

	public bool Local = false;

	Vector3 InitPos;
	float ttimer = 0;

	void Start () {
		InitPos = transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () {
		ttimer += Time.deltaTime;
		Vector3 CurAdd =  new Vector3 (XCurve.Evaluate(ttimer),YCurve.Evaluate(ttimer),ZCurve.Evaluate(ttimer))*ValuesMult;
		if (Local) {
			transform.localPosition = InitPos + (transform.rotation*CurAdd);
		} else {
			transform.localPosition = InitPos + CurAdd;
		}
	}
}

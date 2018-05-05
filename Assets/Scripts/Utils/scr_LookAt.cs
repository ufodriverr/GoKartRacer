using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class scr_LookAt : MonoBehaviour {

	public GameObject Target;

	void Update () {
		if (Target) {	
			transform.LookAt (Target.transform.position);
		} else {
			transform.LookAt (Camera.main.transform.position);
		}
	}
}

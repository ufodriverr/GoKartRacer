using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CratesSpawner : MonoBehaviour {

	public Transform Crate;

	Transform MyCrate;
	float ttimer = 99;

	void Update () {
		if (MyCrate == null) {
			ttimer += Time.deltaTime;
			if (ttimer > 5) {
				ttimer = 0;
				MyCrate = Instantiate (Crate, transform.position, Crate.rotation);
			}
		}
	}
}

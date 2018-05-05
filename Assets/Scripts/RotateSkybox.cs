using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class RotateSkybox : MonoBehaviour {

	public Material Skybox;

	float Value = 0;

	void Update () {
		if (Skybox != null) {
			Value += Time.deltaTime;
			if (Value > 360) {
				Value = 0;
			}
			Skybox.SetFloat ("_Rotation",Value);
		}
	}
}

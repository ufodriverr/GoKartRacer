using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTexture : MonoBehaviour {

	public Vector2 Speed;
	public Material mat;

	void Update () {
		if (mat) {
			mat.mainTextureOffset += Speed * Time.deltaTime;
		}
	}
}

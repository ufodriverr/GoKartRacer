using UnityEngine;
using System.Collections;

public class scr_DestoryAfterSec : MonoBehaviour {

	public float Secs = 0;
	float _timer=0;

	void Update () {
		_timer += Time.deltaTime;
		if (_timer > Secs) {
			Destroy (gameObject);
		}
	}
}

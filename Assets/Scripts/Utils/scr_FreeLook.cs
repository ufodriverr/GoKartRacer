using UnityEngine;
using System.Collections;

public class scr_FreeLook : MonoBehaviour {

	public float CamSpeed = 1;
	public float CamRotSpeed = 1;
	float Height = 0;
	
	void Update () {
		transform.Translate (new Vector3(Input.GetAxis("Horizontal")*CamSpeed,0,Input.GetAxis("Vertical")*CamSpeed)*Time.deltaTime);
		Height = BoolToInt(Input.GetKey(KeyCode.E))-BoolToInt(Input.GetKey(KeyCode.Q));
		transform.position += new Vector3 (0,Height*CamSpeed,0)*Time.deltaTime;
		if (Input.GetMouseButton (1)) {
			Cursor.lockState = CursorLockMode.Locked;
			transform.Rotate (new Vector3(-Input.GetAxis("Mouse Y")*CamRotSpeed,0,0),Space.Self);
			transform.Rotate (new Vector3(0,Input.GetAxis("Mouse X")*CamRotSpeed,0),Space.World);
		} else {
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
	}

	int BoolToInt(bool Input){
		if (Input) {
			return 1;
		} else {
			return 0;
		}
	}
}

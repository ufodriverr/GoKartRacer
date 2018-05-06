using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LapsCounter : MonoBehaviour {

	public TextMeshProUGUI LapsText;
	public TextMeshProUGUI CurTime;
	float ttimer = 0;
	int Circle = 1;
	bool counting = false;
	float lastTime;

	void Start(){
		LapsText.text = "";
		counting = false;
	}

	void Update(){
		if (Input.GetKeyDown (KeyCode.T)) {
			LapsText.text = "";
			counting = false;
			Circle = 1;
		}
		if (counting) {
			CurTime.text = (Time.time - lastTime).ToString ("###.##") + " sec";
		}
	}

	void OnTriggerEnter(Collider coll){
		if (!coll.CompareTag ("Player")) {
			return;
		}
		if (counting) {
			AddLap ();
		} else {
			counting = true;
			lastTime = Time.time;
		}
	}

	void AddLap(){
		LapsText.text += "" + Circle + ":" + (Time.time - lastTime).ToString("###.##")+"\n";
		lastTime = Time.time;
		Circle++;
	}
}

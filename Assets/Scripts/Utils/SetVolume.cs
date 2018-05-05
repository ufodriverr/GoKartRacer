using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetVolume : MonoBehaviour {

	public float Volume = 1;
	AudioSource AS;
	void Start(){
		AS = GetComponent<AudioSource> ();
	}

	void FixedUpdate(){
		if (AS) {
			AS.volume = Volume * Global.Instance.SFXVolume;
		} else {
			Destroy (this);
		}
	}

}

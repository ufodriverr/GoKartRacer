using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(KartReferences))]
public class KartInputs : MonoBehaviour {

	[Header("Input")]
	public KeyCode ForwardButton = KeyCode.W;
	public KeyCode BackButton = KeyCode.S;
	public KeyCode TurnRightButton = KeyCode.D;
	public KeyCode TurnLeftButton = KeyCode.A;
	public KeyCode BrakeButton = KeyCode.LeftControl;
	public KeyCode JumpButton = KeyCode.Space;
	public KeyCode BoostButton = KeyCode.LeftShift;

	KartReferences KartRef;
	void Awake(){
		KartRef = GetComponent<KartReferences> ();
		KartRef.KartInputs = this;
	}
}

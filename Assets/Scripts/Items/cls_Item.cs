using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class cls_Item : MonoBehaviour {

	public KartReferences KartRef;
	public float LongTime;
	public float BoostPercent;
	public float InitAcceleration;

	public abstract void UseItem ();

}

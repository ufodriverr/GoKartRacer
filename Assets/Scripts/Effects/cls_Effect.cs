using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cls_Effect : MonoBehaviour {

	public cls_Effect.Effect_Type Type = cls_Effect.Effect_Type.Boost;

	public float LongTime = 0;

	public float BoostPercent = 1;

	public float InitAcceleration = 0;
	public float ttimer = 0;

	public enum Effect_Type{
		Boost,
		Knockout,
		SlowDown,
		SlipperyTires,
		LostControll
	}

}

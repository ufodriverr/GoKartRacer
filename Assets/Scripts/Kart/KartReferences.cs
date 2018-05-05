using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KartReferences : MonoBehaviour {

	public KartInputs KartInputs;
	public KartParams KartParams;
	public KartEffects KartEffects;
	public KartPhysics KartPhysics;
	public KartItem KartItem;

	public Rigidbody RigidBody;
	public Animator Animator;
	public Transform[] Wheels;
	public Transform KartVisual;
	public TrailRenderer[] Trail;
	public Text SpeedText;
	public Image BoostVisual;
	public AnimationClip LostControlAnimation;
	public Transform PlayerSpawn;
	public AudioSource KartEngineSFX;
	public AudioSource KartTireDriftSFX;

}

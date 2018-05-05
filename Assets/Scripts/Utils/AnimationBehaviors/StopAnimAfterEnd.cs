using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopAnimAfterEnd : StateMachineBehaviour {

	float Secs = 0;

	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		animator.SetInteger ("State",0);
		Secs = 0;
	}

	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		Secs += Time.deltaTime;
		if (Secs >= stateInfo.length) {
			//Debug.Log ("RestartAnim");
			animator.Play(stateInfo.shortNameHash,-1,0);
		}
	}
}

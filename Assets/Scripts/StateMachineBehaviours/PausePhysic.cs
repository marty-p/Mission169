using UnityEngine;
using System.Collections;

public class PausePhysic : StateMachineBehaviour {
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.GetComponent<SlugPhysics>().enabled = false;
	}

	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.GetComponent<SlugPhysics>().enabled = true;
	}
}

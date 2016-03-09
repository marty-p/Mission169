using UnityEngine;
using System.Collections;

public class DisableAnimator : StateMachineBehaviour {

	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.enabled = false;
        //animator.gameObject.enabled = false;
	}

}

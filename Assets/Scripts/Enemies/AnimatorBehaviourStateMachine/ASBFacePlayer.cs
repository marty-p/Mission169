using UnityEngine;
using System.Collections;

public class ASBFacePlayer : StateMachineBehaviour {

    private AnimDrivenBrain brain;

	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (brain == null) {
            brain = animator.GetComponent<AnimDrivenBrain>();
        }
        brain.FaceTarget();
	}

}

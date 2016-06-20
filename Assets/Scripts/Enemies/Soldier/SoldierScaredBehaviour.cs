using UnityEngine;

public class SoldierScaredBehaviour : StateMachineBehaviour {

    private AnimDrivenBrain brain;

	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	    if (brain == null) {
            brain = animator.GetComponent<AnimDrivenBrain>();
        }
	}

	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (UnityEngine.Random.value < brain.getScaredFactor) {
            animator.SetTrigger("run_away");
        }
	}

}

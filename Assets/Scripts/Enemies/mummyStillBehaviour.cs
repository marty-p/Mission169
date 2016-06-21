using UnityEngine;
using System.Collections;

public class mummyStillBehaviour : StateMachineBehaviour {

    private AnimDrivenBrain brain;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	    if (brain == null) {
            brain = animator.GetComponent<AnimDrivenBrain>();
        }
	}

	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	    //if (brain.ShouldIWalk()) { 
            animator.SetBool("walking", true);
        //} else if (brain.ShouldIAttack()) {
            animator.SetTrigger("attack");

        //}
	}


}

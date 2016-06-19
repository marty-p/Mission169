using UnityEngine;
using System.Collections;

public class LaughAnimBeheaviour : StateMachineBehaviour {

    private AnimDrivenBrain brain;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {        
	    if (brain == null) {
            brain = animator.GetComponent<AnimDrivenBrain>();
        }

        animator.SetFloat("laugh_change", 0);
    }
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        float val = UnityEngine.Random.value;
        // Don't want to switch laughing type if we're in CD
      //  if (!brain.laughCD.enabled) {
            animator.SetFloat("laugh_change", val);
     //   }
    }

	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.SetFloat("laugh_change", 0);
        //brain.laughCD.enabled = true;
	}


}

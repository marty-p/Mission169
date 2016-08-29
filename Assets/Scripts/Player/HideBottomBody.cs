using UnityEngine;
using System.Collections;

public class HideBottomBody : StateMachineBehaviour {

    private HideBottomBodyPart hideBottomScript;
    public bool leaveHiddenOnExit;

	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (hideBottomScript == null) {
            hideBottomScript = animator.GetComponent<HideBottomBodyPart>();	
        }
        hideBottomScript.HideBottomBody();
	}

	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	    if (hideBottomScript == null) {
            hideBottomScript = animator.GetComponent<HideBottomBodyPart>();	
        }
        if (!leaveHiddenOnExit) {
            hideBottomScript.ShowBottomBody();
        }
	}

}

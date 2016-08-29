using UnityEngine;
using System.Collections;

public class ShowBottomBody : StateMachineBehaviour {

    private HideBottomBodyPart hideBottomScript;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (hideBottomScript == null) {
            hideBottomScript = animator.GetComponent<HideBottomBodyPart>();	
        }
        hideBottomScript.ShowBottomBody();
	}
}

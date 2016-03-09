using UnityEngine;
using System.Collections;

public class BlinkAtTheEnd : StateMachineBehaviour {

    private Blink blink;

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (blink == null) {
            blink = animator.GetComponent<Blink>();
        }
        blink.BlinkPlease();
	}

}

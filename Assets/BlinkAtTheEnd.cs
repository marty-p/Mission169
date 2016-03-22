using UnityEngine;
using System.Collections;

public class BlinkAtTheEnd : StateMachineBehaviour {

    private Blink blink;

	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (blink == null) {
            blink = animator.GetComponent<Blink>();
        }

        Blink.EndCB desactivateGameObject = () => {
            animator.gameObject.SetActive(false);
        };

            
        animator.enabled = false;
        blink.SetEndCB(desactivateGameObject);
        blink.BlinkPlease();
	}

}

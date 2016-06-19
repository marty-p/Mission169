using UnityEngine;
using System.Collections;

public class ShowBottomBody : StateMachineBehaviour {

    private SpriteRenderer sr;
    private SimulateHovering utils;

	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (utils == null) {
            utils = animator.transform.GetComponentInParent<SimulateHovering>();
        }
        utils.HideBottom();
	}

	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (utils == null) {
            utils = animator.transform.GetComponentInParent<SimulateHovering>();
        }
        utils.ShowBottom();
    }
}

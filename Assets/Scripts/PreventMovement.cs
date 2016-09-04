using UnityEngine;
using System.Collections;
using System;

public class PreventMovement : StateMachineBehaviour {

    private MovementManager moveManager;

	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (moveManager == null) {
           moveManager = animator.GetComponentInParent<MovementManager>();
        }
        moveManager.BlockMovement();
	}

	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	    if (moveManager == null) {
           moveManager = animator.GetComponentInParent<MovementManager>();
        }
        moveManager.AllowMovement();
	}

}

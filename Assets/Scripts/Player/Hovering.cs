using UnityEngine;
using System.Collections;

public class Hovering : StateMachineBehaviour {

	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        SimulateHovering simulate = animator.transform.GetComponentInParent<SimulateHovering>();
        simulate.hoveringOn();
    }

	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	    SimulateHovering simulate = animator.transform.GetComponentInParent<SimulateHovering>();
        simulate.hoveringOff();
	}

}

using UnityEngine;
using System.Collections;

public class ASBGrenadeDeath : StateMachineBehaviour {

    private SlugPhysics physics;

	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (physics == null) {
            physics = animator.GetComponent<SlugPhysics>();
        }
        physics.SetVelocity(0.5f, 10.5f);
        physics.JumpHighVel();
        Debug.Log("CALLED!!!!!");
	}

}

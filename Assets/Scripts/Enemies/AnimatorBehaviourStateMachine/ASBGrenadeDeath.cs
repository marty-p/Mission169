using UnityEngine;
using System.Collections;

public class ASBGrenadeDeath : StateMachineBehaviour {

    private PhysicsSlugEngine physics;

	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (physics == null) {
            physics = animator.GetComponent<PhysicsSlugEngine>();
        }
        physics.SetVelocity(0.2f, 3.5f);
	}

}

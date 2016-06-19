using UnityEngine;
using System.Collections;

public class SwitchToRedScaleShader : StateMachineBehaviour {

    public Material redScaleMaterial;
    private SpriteRenderer spriteRenderer;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        spriteRenderer = animator.GetComponent<SpriteRenderer>();
        spriteRenderer.material = redScaleMaterial;
	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	//override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}

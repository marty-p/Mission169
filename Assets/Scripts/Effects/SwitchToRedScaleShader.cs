using UnityEngine;
using System.Collections;

public class SwitchToRedScaleShader : StateMachineBehaviour {

    public Material redScaleMaterial;
    private Material initialMaterial;
    private SpriteRenderer spriteRenderer;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        spriteRenderer = animator.GetComponent<SpriteRenderer>();
        initialMaterial = spriteRenderer.material;
        spriteRenderer.material = redScaleMaterial;
	}

	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        spriteRenderer.material = initialMaterial;
	}
}

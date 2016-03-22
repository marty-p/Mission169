using UnityEngine;

public class DesactivateGameObject : StateMachineBehaviour {

	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.gameObject.SetActive(false);
	}

}

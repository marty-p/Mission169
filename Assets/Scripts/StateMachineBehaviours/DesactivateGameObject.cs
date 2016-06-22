using UnityEngine;

public class DesactivateGameObject : StateMachineBehaviour {

	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        Transform t = animator.transform;
        while (t.parent != null && t.parent.tag == "enemy") {
            t = t.parent;
        }
        t.gameObject.SetActive(false);
	}

}

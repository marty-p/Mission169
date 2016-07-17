using UnityEngine;

public class DesactivateGameObject : StateMachineBehaviour {

    public bool onExit = true;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (!onExit) {
            Desactivate(animator.transform);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (onExit) {
            Desactivate(animator.transform);
        }
    }

    private void Desactivate (Transform t) {
        while (t.parent != null && t.parent.tag == "enemy") {
            t = t.parent;
        }
        t.gameObject.SetActive(false);
    }

}

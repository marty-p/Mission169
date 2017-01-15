using UnityEngine;

public class ASBSoldierLaugh : StateMachineBehaviour {

    private float timeToLaughThisWay;
    private float startTime;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        UnityEngine.Random.InitState((int)(animator.transform.position.x * Mathf.Pow(10,6)));
        timeToLaughThisWay = UnityEngine.Random.Range(0.5f, 2.5f);
        startTime = Time.time;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (Time.time - startTime > timeToLaughThisWay) {
            UnityEngine.Random.InitState((int)(Time.time*Mathf.Pow(10,6)));
            var val = UnityEngine.Random.value;
            if (val > 0.5f) {
                animator.SetTrigger("laugh_change");
            } else {
                animator.SetTrigger("laugh_change2");
            }
        }
    }

}

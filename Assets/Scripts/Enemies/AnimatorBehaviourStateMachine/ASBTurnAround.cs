using UnityEngine;

public class ASBTurnAround : StateMachineBehaviour {

	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            EnemyUtils.EnemyMovement.TurnAround(animator.transform);
	}

}

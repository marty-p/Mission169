using Slug.StateMachine;
using UnityEngine;

public class SoldierRunAwayBehaviour : Room {

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        brain.TurnBackToTarget();
    }

	override public void Update(Animator animator) {
        brain.Walk(50);
	}

}

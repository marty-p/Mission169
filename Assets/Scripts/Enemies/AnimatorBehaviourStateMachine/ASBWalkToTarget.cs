using Slug.StateMachine;
using UnityEngine;

public class ASBWalkToTarget : Room {

    private float distanceToStop;
    public int walkingSpeed = 20;

    public override void Init() {
        Door toStill = new Door(() => animator.SetBool("walking", false),
                                () => brain.GetAbsTargetDistance() < distanceToStop);
        AddExitDoor(toStill);
    }   

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        distanceToStop = brain.distanceStopWalking;
    }

    public override void Update() {
        brain.WalkToTarget(walkingSpeed);
    }
}

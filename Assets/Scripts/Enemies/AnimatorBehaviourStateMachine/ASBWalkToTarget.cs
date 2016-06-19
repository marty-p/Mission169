using Slug.StateMachine;
using UnityEngine;

public class ASBWalkToTarget : Room {

    private float  distanceToStop;
    public int walkingSpeed = 20;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        distanceToStop = UnityEngine.Random.Range(0.6f, 1.9f);
    }

    public override void Init(Animator anim) {
        Door toStill = new Door(()=>anim.SetBool("walking", false),
                                ()=>brain.GetAbsTargetDistance() < distanceToStop);
        AddExitDoor(toStill);
    }

    public override void Update(Animator anim) {
        brain.WalkToTarget(walkingSpeed);
    }
}

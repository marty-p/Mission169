using System;
using Slug.StateMachine;
using UnityEngine;

public class ASBWalkAwayFromTarget : Room {

    public float stopDistance = 1;

    public override void Init(Animator anim) {
        Door toStill = new Door(()=>anim.SetBool("walking", false),
                                ()=>brain.GetAbsTargetDistance() > stopDistance);
        AddExitDoor(toStill);
    }

    public override void Enter(Animator animator) {
        stopDistance = UnityEngine.Random.Range(1, 1.5f);
    }

    public override void Update(Animator anim) {
        brain.WalkAwayFromTarget(stopDistance);
    }
}

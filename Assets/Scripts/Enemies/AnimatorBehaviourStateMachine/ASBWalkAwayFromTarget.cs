using System;
using Slug.StateMachine;
using UnityEngine;

public class ASBWalkAwayFromTarget : Room {

    public float stopDistance = 1;

    public override void Init() {
        Door toStill = new Door(()=>animator.SetBool("walking", false),
                                ()=>brain.GetAbsTargetDistance() > stopDistance);
        AddExitDoor(toStill);
    }

    public override void Enter() {
        stopDistance = UnityEngine.Random.Range(1, 1.5f);
    }

    public override void Update() {
        brain.WalkAwayFromTarget(stopDistance);
    }
}

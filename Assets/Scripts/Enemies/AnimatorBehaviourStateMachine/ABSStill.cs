using UnityEngine;
using Slug.StateMachine;

public class ABSStill : Room {

    public override void Init(Animator anim) {
        Door toGrenade = new Door(()=>anim.SetTrigger("grenade_standing"),
                ()=> brain.TargetDistBetween(0.6f, 2), 3);
        AddExitDoor(toGrenade);

        Door toWalk = new Door(()=>OnGoingToWalk(), ()=>brain.TargetDistMoreThan(2), 0);
        AddExitDoor(toWalk);

        Door toWalkBack = new Door(()=>anim.SetBool("walking_away_from_target", true), 
                ()=> brain.TargetDistBetween(0.3f, 0.8f), 5) ;
        AddExitDoor(toWalkBack);

        Door toKnife = new Door(()=>anim.SetTrigger("knife"), ()=>brain.TargetDistLessThan(0.4f), 2.5f);
        AddExitDoor(toKnife);

        Door toBodyContact = new Door(()=>OnGoingToWalk(0.3f), ()=>brain.TargetDistLessThan(1.5f), 1);
        AddExitDoor(toBodyContact);
    }

    private void OnGoingToWalk(float distanceToStop = 0) {
        if (distanceToStop == 0) {
            distanceToStop = UnityEngine.Random.Range(1, 1.9f);
        }
        animator.SetTrigger("walking");
        brain.distanceStopWalking = distanceToStop;
    }

}

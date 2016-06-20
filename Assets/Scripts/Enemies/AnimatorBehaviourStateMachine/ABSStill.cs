using UnityEngine;
using Slug.StateMachine;

public class ABSStill : Room {

    public override void Init(Animator anim) {
        Door toGrenade = new Door(()=>anim.SetTrigger("grenade_standing"), brain.ShouldIGrenade, 3);
        AddExitDoor(toGrenade);

        Door toWalk = new Door(()=>anim.SetBool("walking", true), brain.ShouldIWalk, 0) ;
        AddExitDoor(toWalk);

        Door toKnife = new Door(()=>anim.SetTrigger("knife"), brain.ShouldISlice, 2.5f) ;
        AddExitDoor(toKnife);

        Door toWalkBack = new Door(()=>anim.SetBool("walking_away_from_target", true), brain.ShouldIWalkBack, 5) ;
        AddExitDoor(toWalkBack);
    }
}

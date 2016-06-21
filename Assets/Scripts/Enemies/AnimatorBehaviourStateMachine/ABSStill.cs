using UnityEngine;
using Slug.StateMachine;

public class ABSStill : Room {

    public override void Init(Animator anim) {
        Door toGrenade = new Door(()=>anim.SetTrigger("grenade_standing"), brain.ShouldIGrenade, 3);
        AddExitDoor(toGrenade);

        Door toWalk = new Door(()=>anim.SetBool("walking", true), ()=> TargetDistMoreThan(2), 0);
        AddExitDoor(toWalk);

        Door toWalkBack = new Door(()=>anim.SetBool("walking_away_from_target", true),
                brain.ShouldIWalkBack, 7) ;
        AddExitDoor(toWalkBack);
        /*
                Door toKnife = new Door(()=>anim.SetTrigger("knife"), brain.ShouldISlice, 2.5f);
                AddExitDoor(toKnife);
        */
        Door toBodyContact = new Door(() => 
            {
                anim.SetBool("walking", true);
                
            },
            () => TargetDistLessThan(1));
    }


    private bool TargetDistLessThan(float dist) {
        float targetDist = brain.GetAbsTargetDistance();

        return targetDist < dist;
    }

    private bool TargetDistMoreThan(float dist) {
        float targetDist = brain.GetAbsTargetDistance();

        return targetDist > dist;
    }

}

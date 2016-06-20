using UnityEngine;
using Slug.StateMachine;

public class ASBSoldierLaugh : Room {

    private float laughingVal;

    public override void Init(Animator anim) {
        Door toOtherLaugh = new Door( ()=> anim.SetTrigger("laugh_change"), ()=> laughingVal > 0.6f, 1f);
        AddExitDoor(toOtherLaugh);
    }

    public override void Update(Animator anim) {
        laughingVal = UnityEngine.Random.value;
    }

}

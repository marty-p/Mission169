using UnityEngine;
using Slug.StateMachine;

public class ASBSoldierLaugh : Room {

    private float laughingVal;

    public override void Init() {
        Door toOtherLaugh = new Door( ()=> animator.SetTrigger("laugh_change"), ()=> laughingVal > 0.6f, 1f);
        AddExitDoor(toOtherLaugh);
    }

    public override void Update() {
        laughingVal = UnityEngine.Random.value;
    }

}

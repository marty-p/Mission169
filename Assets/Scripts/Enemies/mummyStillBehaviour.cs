using UnityEngine;
using Slug.StateMachine;

public class mummyStillBehaviour : Room {

    public override void Init() {
        Door toWalking = new Door(
                () => animator.SetBool("walking", true),
                () => brain.TargetDistMoreThan(0.5f)
        );
        AddExitDoor(toWalking);
    }

}

using UnityEngine;

public class MovementManager : MonoBehaviour, IObserver {
    private PhysicsSlugEngine physics;

    public void Observe(SlugEvents ev) {
        if (ev == SlugEvents.MovingRight) {
            physics.changeDirection(Vector2.right);
            physics.MoveForward();
        } else if ( ev == SlugEvents.MovingLeft) {
            physics.changeDirection(Vector2.left);
            physics.MoveForward();
        } else if (ev == SlugEvents.Jump) {
            physics.Jump();
        } else if (ev == SlugEvents.Sit && !physics.InTheAir) {
            physics.SetMovementFactor(0.2f);
        } else if (ev == SlugEvents.Stand && !physics.InTheAir) {
            physics.SetMovementFactor(0.8f);
        }
    }

    void Awake () {
        physics = gameObject.AddComponent<PhysicsSlugEngine>();
	}

}

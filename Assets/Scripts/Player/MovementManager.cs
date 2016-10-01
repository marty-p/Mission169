using System.Collections;
using UnityEngine;

public enum LookDirection {Straight, Up, Down}
public enum BodyPosture {Stand, Running, InTheAir, Crouch}

public class MovementManager : MonoBehaviour, IObserver {

    public Vector2 lookingDirection;
    public BodyPosture body;
    private TimeUtils timeUtils;

    private SlugPhysics physics;
    private AnimationManager animManager;
    private IObserver[] observers;
    private BoxCollider2D collider;

    public float crouchSpeedFactor = 0.25f;

    public void Observe(SlugEvents ev) {
        if (ev == SlugEvents.HitGround) {
            if (lookingDirection == Vector2.down) {
                Crouch();
            }
        }
    }

    private void TurnAround() {
         physics.ChangeDirection(-transform.right);
        if (lookingDirection != Vector2.up && lookingDirection != Vector2.down) {
            animManager.StartTurnAnim();
            lookingDirection = transform.right;
        }
    }
            
    public void HorizontalMovement(Vector3 dir) {
        if (transform.right != dir) {
            TurnAround();
        }
        if (physics.InTheAir) {
            physics.SetVelocityX(dir.x);
        }
        physics.SetForceX(dir.x);
        animManager.StartRunningAnim();
    }

    public void StopMoving() {
        physics.SetForceX(0);
        animManager.StopRunningAnim();
    }

    public void Jump() {
        if (body == BodyPosture.Crouch) {
            LookDown();
        }
        body = BodyPosture.Stand;
        if (Mathf.Abs(physics.GetVelocityX()) > 0) {
            if (physics.JumpHighVel()) {
                timeUtils.FrameDelay(animManager.StartHighVelJumpAnim);
            }
        } else {
            if(physics.JumpLowVel()) {
                timeUtils.FrameDelay(animManager.StartLowVelJumpAnim);
            }
        }
        AdaptColliderStanding();
    }

    public void LookUp() {
        lookingDirection = Vector2.up;
        animManager.StartLookUpAnim();
    }

    private void LookDown() {
        lookingDirection = Vector2.down;
        animManager.StartLookDownAnim();
    }

    public void DefaultBodyPosition() {
        body = BodyPosture.Stand;
        lookingDirection = transform.right;
        animManager.StartLookStraightAnim();
        physics.SetMovementFactor(physics.groundMovementFactor);
        AdaptColliderStanding();
    }

    public void DownMovement() {
        if (physics.InTheAir) {
            LookDown();
        } else if (body == BodyPosture.Stand) {
            Crouch();
        }
    }

    private void Crouch() {
        body = BodyPosture.Crouch;
        lookingDirection = transform.right;
        animManager.StartCrouchAnim();
        physics.SetMovementFactor(crouchSpeedFactor);
        AdaptColliderCrouching();
    }

    public void BlockMovement() {
        physics.SetMovementFactor(0);
    }

    public void AllowMovement() {
        if (body == BodyPosture.Crouch) {
            physics.SetMovementFactor(crouchSpeedFactor);
        } else if (body == BodyPosture.Stand) {
            physics.SetMovementFactor(physics.groundMovementFactor);
        } 
    }

    public bool IsInMotion() {
        return physics.GetVelocity() != Vector2.zero;
    }

    void Awake () {
        physics = gameObject.GetComponent<SlugPhysics>();
        animManager = GetComponent<AnimationManager>();
        timeUtils = GetComponent<TimeUtils>();
        collider = GetComponent<BoxCollider2D>();
	}

    void NotifyObservers(SlugEvents ev) {
        if (observers == null) {
            observers = GetComponents<IObserver>();
        }
        foreach(IObserver obs in observers) {
            obs.Observe(ev);
        }
    }

    IEnumerator WaitAFrameAndTurnRight() {
        yield return 0;
        physics.ChangeDirection(Vector2.right);
    }

    IEnumerator WaitAFrameAndTurnLeft() {
        yield return 0;
        physics.ChangeDirection(Vector2.left);
    }

    private void AdaptColliderCrouching() {

        float new_size = collider.size.y/2;
        float diff = collider.size.y - new_size;
        collider.offset = new Vector2(collider.offset.x, collider.offset.y - diff/2);
        collider.size = new Vector2(collider.size.x, new_size);
    }

    private void AdaptColliderStanding() {
        collider.offset = new Vector2(0, 0.09f);
        collider.size = new Vector2(0.16f, 0.357f);
    }

}

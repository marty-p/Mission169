using UnityEngine;
using Utils;

public class AnimationManager : MonoBehaviour, IObserver {

    public Animator topAnimator;
    public Animator bottomAnimator;
    public Animator blood;

    private RetVoidTakeVoid EndOfDeathCB;
    private bool inExplosiveDeathAnim;

    public RetVoidTakeVoid grenadeCB;

    public void StartRunningAnim() {
        topAnimator.SetBool("horizontal_pressed", true);
        bottomAnimator.SetBool("horizontal_pressed", true);
    }

    public void StopRunningAnim() {
        topAnimator.SetBool("horizontal_pressed", false);
        bottomAnimator.SetBool("horizontal_pressed", false);
    }

    public void StartLowVelJumpAnim() {
        bottomAnimator.SetTrigger("jump_low_speed");
        topAnimator.SetTrigger("jump_trigger");
        topAnimator.SetBool("jump_low_speed", true);
    }

    public void StartHighVelJumpAnim() {
        topAnimator.SetTrigger("jump_trigger");
        bottomAnimator.SetTrigger("jump_high_speed");
        topAnimator.SetBool("jump_high_speed", true);
    }

    public void StartTurnAnim() {
        topAnimator.SetTrigger("turn");
    }

    public void StartLookUpAnim() {
        if (!topAnimator.GetBool("up_pressed")) {
            if (!topAnimator.GetBool("jump_low_speed") 
                    && !topAnimator.GetBool("jump_high_speed") 
                    && !topAnimator.GetBool("down_pressed") ) {
                topAnimator.SetTrigger("look_up_trigger");
            }
        } 
        topAnimator.SetBool("up_pressed", true);
    }

    public void StartLookStraightAnim() {
        topAnimator.SetBool("down_pressed", false);
        bottomAnimator.SetBool("down_pressed", false);
        topAnimator.SetBool("up_pressed", false);
    }

    public void StartCrouchAnim() {
        topAnimator.SetTrigger("sit");
        topAnimator.SetBool("down_pressed", true);
        bottomAnimator.SetBool("down_pressed", true);
    }

    public void StartLookDownAnim() {
        topAnimator.SetBool("down_pressed", true);
    }

    public void StartStandingUpAnim() {
        StartLookStraightAnim();
    }

    public void Observe(SlugEvents ev) {
        if (!topAnimator.isInitialized) {
            return;
        }

        if (ev == SlugEvents.Fall && !inExplosiveDeathAnim) {
            topAnimator.SetTrigger("jump_trigger");
            topAnimator.SetBool("jump_low_speed", true);
            bottomAnimator.SetTrigger("jump_low_speed");
        } else if (ev == SlugEvents.HitGround) {
            if (inExplosiveDeathAnim) {
                topAnimator.applyRootMotion = false;
                topAnimator.SetTrigger("touch_ground_death");
                return;
            }
            topAnimator.SetTrigger("hit_ground");
            bottomAnimator.SetTrigger("hit_ground");
            topAnimator.SetBool("jump_low_speed", false);
            topAnimator.SetBool("jump_high_speed", false);
        }
    }

    public void StartGrenadeAnim(RetVoidTakeVoid cb) {
        topAnimator.SetTrigger("grenade");
        grenadeCB = cb;
    }

    public void PlaySpawnAnim() {
        topAnimator.Play("marco-spawn");
    }

    public void PlayDeathAnimation(ProjectileProperties proj, RetVoidTakeVoid cb) {
        string trigger;

        if (proj.type == ProjectileType.Grenade) {
            trigger = "death_explosive";
            inExplosiveDeathAnim = true;
        } else if (proj.type == ProjectileType.Knife) {
            trigger = "slashed";
            blood.Play("1");
        } else {
            trigger = "slashed";
        }
        EndOfDeathCB = cb;
        topAnimator.SetTrigger(trigger);
    }

    public void EndOfDeathAnim() {
        if (EndOfDeathCB != null) {
            EndOfDeathCB();
        }
    }

    public void MissionCompleteAnim() {
        topAnimator.SetTrigger("mission_complete");
    }

    public void ResetTopAnimator() {
        topAnimator.Play("just_chillin");
        inExplosiveDeathAnim = false;
    }

    public void ResetBottomAnimator() {
        bottomAnimator.Play("legs_still");
    }

}

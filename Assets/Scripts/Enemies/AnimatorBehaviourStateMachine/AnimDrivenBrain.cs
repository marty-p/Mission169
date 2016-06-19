using UnityEngine;
using System;
using Utils;
using Slug.StateMachine;

public class AnimDrivenBrain : MonoBehaviour {

    private Animator anim;
    private Transform target;
    private float distanceToTarget;
    private PhysicsSlugEngine physic;
    private float absDistanceToTarget;

    public float getScaredFactor = 0.5f;

    void Awake () {
        anim = GetComponent<Animator>();
        physic = GetComponent<PhysicsSlugEngine>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }
	
	void Update () {
        UpdateDistanceToTarget();
    }

    private void UpdateDistanceToTarget() {
        distanceToTarget = transform.position.x - target.position.x;
        absDistanceToTarget = Mathf.Abs(distanceToTarget);
        print(absDistanceToTarget);
    }

    public bool ShouldIWalk() {
        return absDistanceToTarget > 2;
    }
    public bool ShouldIGrenade() {
        return absDistanceToTarget < 2 && absDistanceToTarget > 0.6f;
    }
    public bool ShouldIAttack() {
        return absDistanceToTarget < 0.5f;
    }
    public bool ShouldIWalkBack() {
        return absDistanceToTarget > 0.30f && absDistanceToTarget < 0.55f;
    }
    public bool ShouldISlice() {
        float distanceFromPlayer = transform.position.x - target.position.x;
        return Math.Abs(distanceFromPlayer) < 0.4f;
    }
    public float GetAbsTargetDistance() {
        return absDistanceToTarget;
    }

    public bool WalkToTarget(float stopDistance, int walkingSpeed) {
        if (absDistanceToTarget < stopDistance) {
            return true;
        } else if (!FacingTarget()) {
            anim.SetTrigger("turn_around");
            return false;
        } else {
            physic.MoveForward(walkingSpeed);
            return false;
        }
    }

    public bool WalkToTarget(int walkingSpeed) {
        if (absDistanceToTarget < 0) {
            return true;
        } else if (!FacingTarget()) {
            anim.SetTrigger("turn_around");
            return false;
        } else {
            physic.MoveForward(walkingSpeed);
            return false;
        }
    }


    public bool WalkAwayFromTarget(float stopDistance) {
        if (FacingTarget()) {
            anim.SetTrigger("turn_around");
            return false;
        } else if (absDistanceToTarget > stopDistance) {
            anim.SetBool("walking_away_from_target", false);
            return true;
        } else {
            physic.MoveForward();
            return false;
        }
    }

    public void FaceTarget() {
        if (!FacingTarget()) {
            EnemyUtils.EnemyMovement.TurnAround(transform);
        }
    }
    public void TurnBackToTarget() {
        if (FacingTarget()) {
            EnemyUtils.EnemyMovement.TurnAround(transform);
        }
    }
    private bool FacingTarget() {
        return Math.Sign(transform.right.x) != Math.Sign(distanceToTarget);
    }

    public void Walk(int speed=0) {
        physic.MoveForward(speed);
    }
}

public struct AnimatorStateCBs {
    public RetVoidTakeVoid Start;
    public RetVoidTakeVoid Update;
    public RetVoidTakeVoid End;
}

public class CoolDown {
    private float duration;
    private float elapsed;
    public bool enabled;

    public CoolDown(float duration) {
        this.duration = duration;
    }

    public void Update() {
        if (elapsed < duration) {
            elapsed += Time.fixedDeltaTime;
        } else {
            Reset();
        }
    }

    public void Reset() {
        elapsed = 0;
        enabled = false;
    } 
}

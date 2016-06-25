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

    public float distanceStopWalking;

    void Awake () {
        anim = GetComponent<Animator>();
        physic = GetComponent<PhysicsSlugEngine>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        print(target);
    }
	
	void Update () {
        UpdateDistanceToTarget();
    }

    private void UpdateDistanceToTarget() {
        distanceToTarget = transform.position.x - target.position.x;
        absDistanceToTarget = Mathf.Abs(distanceToTarget);
    }

    public bool TargetDistBetween(float distMin, float distMax) {
        if(distMin > distMax) {
            throw new ArgumentException();
        }
        return absDistanceToTarget > distMin && absDistanceToTarget < distMax;
    }

    public bool TargetDistMoreThan(float dist) {
        return absDistanceToTarget > dist;
    }   

    public bool TargetDistLessThan(float dist) {
        return absDistanceToTarget < dist;
    }    

    public float GetAbsTargetDistance() {
        return absDistanceToTarget;
    }

    public bool WalkToTarget(float stopDistance, int walkingSpeed) {
        if (absDistanceToTarget < stopDistance) {
            print(" problem here");
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

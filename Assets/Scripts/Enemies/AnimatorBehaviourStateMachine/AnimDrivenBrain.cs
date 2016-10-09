using UnityEngine;
using System;
using Utils;
using Slug.StateMachine;

public class AnimDrivenBrain : MonoBehaviour {

    private Animator anim;
    private Transform target;
    private float distanceToTarget;
    private SlugPhysics physic;
    private float absDistanceToTarget;
    public bool visible;

    public float getScaredFactor = 0.5f;

    public float distanceStopWalking;

    void Awake () {
        anim = GetComponent<Animator>();
        physic = GetComponent<SlugPhysics>();
    }

    void OnEnable() {
        visible = false;
    }

    void OnBecameVisible() {
        visible = true;
    }

	void OnBecameInvisible() {
        visible = false;
    }

	void Update () {
        if (target == null) {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
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

    public bool WalkToTarget(float stopDistance, float walkingSpeed) {
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

    public bool WalkToTarget(float walkingSpeed) {
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

    public void Walk(float speed=0) {
        physic.MoveForward(speed);
    }
}

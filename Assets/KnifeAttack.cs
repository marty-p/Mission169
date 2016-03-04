using UnityEngine;
using System;

public class KnifeAttack : MonoBehaviour, IEnemyBehavior, ISight {

    public Animator animator;
    private Transform player;
    private float distanceFromPlayer = 10000; 
    private bool attackInProgress;
    public float distanceIGetMyKnifeOut = 0.5f;
    public int behaviorPriority = 3;

    private bool resting = false;
    private float restingTime = 2;
    private float hasRestedFor = 0;

    public void OnPlayerSpotted (Transform transform) {
        if (player==null) {
            player = transform;
        }
    }

	void Update () {
        if (player != null) {
            distanceFromPlayer = transform.position.x - player.position.x;
        }
        if (resting) {
            hasRestedFor =  hasRestedFor + Time.deltaTime;
            if (hasRestedFor >= restingTime) {
                hasRestedFor = 0;
                resting = false;
            }
        }
	}

    void OnKnifeAttackIsOver() {
        attackInProgress = false;
        resting = true;
    }

    void OnKnifeIsOut () {

    }

    public bool WantToStart() {
        if (Math.Abs(distanceFromPlayer) < distanceIGetMyKnifeOut 
                && !resting) {
            return true;
        } else {
            return false;
        }
    }

    public void StartBehavior() {
        attackInProgress = true;
        animator.SetBool("walking", false);
        animator.SetTrigger("knife");
    }

    public bool UpdateBehavior() {
        // Do Nothing
        return true;
    }

    public bool CanBeInterrupted() {
        return false;
    }

    public bool InProgress() {
        return attackInProgress;
    }

    public int GetPriority() {
        return behaviorPriority;
    }
}

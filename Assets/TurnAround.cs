using UnityEngine;
using System;
using EnemyUtils;

public class TurnAround : MonoBehaviour, IEnemyBehavior {

    private Transform player;

    public Rigidbody2D rigidBody;
    private Animator animator;

    public bool inProgress = false;

    public int priority = 2;
    public float behaviorLikelihood =  0.9f;
    public float turnAroundDist = 1;
    private float distanceMaxAwayFromPlayer = 2;

    private float distanceFromPlayer = 1000;

    private bool resting = false;
    private float restingTime = 1;
    private float hasRestedFor = 0;

    private System.Random random;

    void Start() {
        animator = GetComponent<Animator>();
        random = new System.Random();
        player = GameObject.FindGameObjectWithTag("Player").transform;
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


    public bool CanBeInterrupted() {
        return false;
    }

    public int GetPriority() {
        return priority;
    }

    public bool InProgress() {
        return inProgress;
    }

    public void StartBehavior() {
        inProgress = true;
        animator.SetTrigger("turn_around");
    }

    public bool UpdateBehavior() {
        return true;
    }

    public bool WantToStart() {
        if ((
                Math.Abs(distanceFromPlayer) < turnAroundDist
                || (Math.Abs(distanceFromPlayer) > distanceMaxAwayFromPlayer && (distanceFromPlayer > 0 && transform.right.x == 1) )
            )
            && !resting ) 
        {
            float behaviorLuck = UnityEngine.Random.value;
            if (behaviorLuck < behaviorLikelihood) {
                return true;
            } else {
                resting = true;
                return false;
            }
        } else {
            return false;
        }
    }


    public void OnTurningAroundDone () {
        EnemyMovement.TurnAround(transform);
        inProgress = false;
        resting = true;
    }
}

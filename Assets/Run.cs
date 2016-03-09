using UnityEngine;
using EnemyUtils;

public class Run : MonoBehaviour, IEnemyBehavior {

    private Transform player;
    public bool inProgress;
    public int priority = 1;
    public bool interruptible = true;
    private float distanceFromPlayer = 10000;
    public Animator animator;
    public PhysicsSlugEngine physicsController;

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update() {
        if (player != null) {
            distanceFromPlayer = transform.position.x - player.position.x;
        }
    }

    public bool CanBeInterrupted() {
        return interruptible;
    }

    public int GetPriority() {
        return priority;
    }

    public bool InProgress() {
        return inProgress;
    }

    public void StartBehavior() {
        inProgress = true;
         animator.SetBool("walking", true);
    }

    public bool UpdateBehavior()
    {
        if (player == null) {
            return false;
        }
        if (distanceFromPlayer < 0.1f && distanceFromPlayer > -0.1f) {
            animator.SetBool("walking", false);
            inProgress = false;
        } else if (distanceFromPlayer < 0) {
           // physicsController.MoveLeft();
        } else if (distanceFromPlayer > 0) {
            //physicsController.changeDirection()
        }
        physicsController.MoveForward();
        return true;
    }

    public bool WantToStart() {
        return player != null;
    }
}

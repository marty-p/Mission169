using UnityEngine;
using EnemyUtils;
using System.Collections;

public class EnemyBaseTasks : MonoBehaviour {

    public Transform Target { get; set; }
    public bool TaskRunning { get; protected set; }
    protected EnemyTargetDistance targetDistance;
    protected SlugPhysics physic;
    public float SpeedFactor { get; set; } 

    void Awake() {
        physic = GetComponent<SlugPhysics>();
        targetDistance = GetComponent<EnemyTargetDistance>();
        SpeedFactor = 1.0f;
    }

    public virtual void Reset() {
        StopAllCoroutines();
        TaskRunning = false;
    }

    public bool WalkToTarget(float stopDistance) {
        StartCoroutine(WalkToTargetTask(stopDistance));
        return true;
    }

    public void StopAll() {
        StopAllCoroutines();
        TaskRunning = false;
    }

    private IEnumerator WalkToTargetTask(float stopDistance) {
        // INIT
        TaskRunning = true;
        UnityEngine.Random.InitState((int) (transform.position.x*Mathf.Pow(10, 6)));
        float randomStopDistance = stopDistance * UnityEngine.Random.Range(0.7f, 1.3f);
        EnemyMovement.FaceTarget(transform, Target);
        // TASK
        while(targetDistance.MoreThan(randomStopDistance)) {
            physic.MoveForward(SpeedFactor);
            yield return new WaitForEndOfFrame();
        }
        // #dodgy - gives time to end the stop walking anim
        yield return new WaitForSeconds(1);
        // DESINIT
        TaskRunning = false;
    }
}

//TODO MOVE THAT TO ANOTHER FILE
//TODO MOVE THAT TO ANOTHER FILE
public class SoldierTasks : EnemyBaseTasks {

    private SoldierAnimationManager animManager;
    private float lastGrenadeTime = 0;
    public float grenadeCoolDownTime = 3f;

    void Start() {
        animManager = GetComponent<SoldierAnimationManager>();
    }

    public bool GrenadeInCoolDown() {
        return (Time.time - lastGrenadeTime) < grenadeCoolDownTime;
    }

    public bool IsAttacking() {
        return GetComponent<EnemyKnifeAttack>().attacking || GetComponent<AttackTarget>().attacking;
    }

    public override void Reset() {
        base.Reset();
        lastGrenadeTime = 0;
        ResetAttackingStatus();
    }

    public bool AttackGrenade() {
        if (!GrenadeInCoolDown()) {
            StartCoroutine(AttackGrenadeTask());
            return true;
        } else {
            return false;
        }
    }

    public bool AttackKnife() {
        StopAllCoroutines();
        StartCoroutine(AttackKnifeTask());
        return true;
    }

    public bool WalkBackward() {
        StopAllCoroutines();
        StartCoroutine(WalkBackwardTask());
        return true;
    }

    private IEnumerator AttackGrenadeTask() {
        TaskRunning = true;
        EnemyMovement.FaceTarget(transform, Target);
        lastGrenadeTime = Time.time;
        GetComponent<AttackTarget>().Execute("Player");

        while (GetComponent<AttackTarget>().attacking) {
            yield return new WaitForEndOfFrame();
        }
        TaskRunning = false;
    }

    private IEnumerator AttackKnifeTask() {
        TaskRunning = true;
        EnemyMovement.FaceTarget(transform, Target);
        GetComponent<EnemyKnifeAttack>().Execute("Player");

        while (GetComponent<EnemyKnifeAttack>().attacking) {
            yield return new WaitForEndOfFrame();
        }
        TaskRunning = false;
    }

    private IEnumerator WalkBackwardTask() {
        TaskRunning = true;
        EnemyMovement.FaceTarget(transform, Target);
        animManager.WalkBackward();
        yield return new WaitForEndOfFrame();
        while (animManager.IsWalkingBackward) {
            //TODO hmmm, moveForward but it goes backward with negative vel
            physic.MoveForward(-0.1f);
            yield return new WaitForEndOfFrame();
        }
        TaskRunning = false;
    }

    private void ResetAttackingStatus() {
        GetComponent<EnemyKnifeAttack>().attacking = false;
        GetComponent<AttackTarget>().attacking = false;
    }
}

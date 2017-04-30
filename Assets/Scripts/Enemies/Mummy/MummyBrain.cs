using UnityEngine;
using System.Collections;

public class MummyBrain : EnemyBrain {

    private EnemyBaseTasks baseTasks;
    private Transform target;
    private EnemyTargetDistance targetDistance;
    private MummyAnimationEvents animManager;

    void Awake() {
        GameObject playerWraper = Mission169.GameManager.Instance.GetPlayer();
        if (playerWraper != null) {
            target = playerWraper.transform.GetChild(0);
        } else {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
        targetDistance = gameObject.AddComponent<EnemyTargetDistance>();
        targetDistance.Target = target;
        animManager = GetComponent<MummyAnimationEvents>();
    }

    void Start() {
        baseTasks = gameObject.AddComponent<EnemyBaseTasks>();
        baseTasks.Target = target;
        baseTasks.SpeedFactor = 0.3f;
    }

    public override void Pause() {
        base.Pause();
        baseTasks.StopAll();
    }

    void Update() {

        if (targetDistance.LessThan(0.4f)) {
            animManager.Attack();
            return;
        }        

        if (baseTasks.TaskRunning) {
            return;
        }

        if (targetDistance.MoreThan(0.37f) && baseTasks.WalkToTarget(0.35f)) {
            return;
        }
    }

}

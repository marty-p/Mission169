using System;
using UnityEngine;

/*
 * Brain's rules:
 * - Return when a Task is started successfuly
 * - 
 * 
 * */

public class SoldierBrainGeneral : EnemyBrain {

    private SoldierTasks soldierTasks;
    private EnemyTargetDistance targetDistance;
    private Transform target;

	void Awake () {
        GameObject playerWraper = Mission169.GameManager.Instance.GetPlayer();
        if (playerWraper != null) {
            target = playerWraper.transform.GetChild(0);
        } else {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }

        targetDistance = gameObject.AddComponent<EnemyTargetDistance>();
        targetDistance.Target = target;
    }

    void Start() {
        soldierTasks = gameObject.AddComponent<SoldierTasks>();
        soldierTasks.Target = target;
    }

    void Update () {
        if (targetDistance.LessThan(0.2f)) {
            soldierTasks.AttackKnife();
            return;
        }

        if (soldierTasks.TaskRunning) {
            return;
        }

        if (targetDistance.MoreThan(1.5f)
            && soldierTasks.WalkToTarget(1.5f)) {
            return;
        } else if (targetDistance.Between(0.2f, 1f)
                && soldierTasks.WalkToTarget(0.19f)) {
            return;
        } else if (UnityEngine.Random.value > 0.5f) {
            soldierTasks.AttackGrenade();
            return;
        } else {
            soldierTasks.WalkBackward();
            return;
        } 

	}

    public override void Pause() {
        base.Pause();
        soldierTasks.StopAll();
    }

    public override void Reset() {
        base.Reset();
        soldierTasks.Reset(); 
    }

}

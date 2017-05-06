using UnityEngine;
using System.Collections;
using System;
using EnemyUtils;

public class MummyBrain : EnemyBrain {

    private MummyAnimationEvents animManager;

    protected override void Init() { 
        animManager = GetComponent<MummyAnimationEvents>();
    }

    public override void Pause() {
        base.Pause();
        //baseTasks.StopAll();
    }

    protected override IEnumerator Think() {
        while (enabled) {

            if (targetDistance.LessThan(0.4f)) {
                yield return StartCoroutine(Attack());
            } else {
                yield return StartCoroutine(WalkToTarget());
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator WalkToTarget() {
        EnemyMovement.FaceTarget(transform, targetDistance.Target);

        while (targetDistance.MoreThan(0.39f)) {
            physic.MoveForward(0.35f);
            yield return 0;
        }
    }

    IEnumerator Attack() {
        EnemyMovement.FaceTarget(transform, targetDistance.Target);

        animManager.Attack();
        yield return new WaitForSeconds(2f);
    }

}

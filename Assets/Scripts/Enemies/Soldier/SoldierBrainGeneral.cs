using System;
using System.Collections;
using UnityEngine;
using EnemyUtils;


public class SoldierBrainGeneral : EnemyBrain {

    EnemyKnifeAttack knifeAttack;

    protected override void Init() {
        knifeAttack = GetComponent<EnemyKnifeAttack>();
    }

    protected override IEnumerator Think() {
        while (enabled) {
            print("think! " + targetDistance.GetAbs() );

            if (targetDistance.LessThan(0.35f)) {
                yield return StartCoroutine(AttackKnife());
            }

            yield return new WaitForSeconds(0.33f);
        }
    }

    IEnumerator AttackKnife() {
        EnemyMovement.FaceTarget(transform, targetDistance.Target);
        knifeAttack.Execute("Player");

        yield return new WaitForSeconds(1.5f);
    }
    /*
        protected override void Think () {


            if (targetDistance.MoreThan(1.5f) && soldierTasks.WalkToTarget(1.5f)) {
                return;
            } else if (targetDistance.Between(0.2f, 1f) && soldierTasks.WalkToTarget(0.19f)) {
                return;
            } else if (UnityEngine.Random.value > 0.5f) {
                soldierTasks.AttackGrenade();
                return;
            } else {
                soldierTasks.WalkBackward();
                return;
            } 
        }
        */
}

using System;
using System.Collections;
using UnityEngine;
using EnemyUtils;

public class SoldierBrainGeneral : EnemyBrain {

    EnemyKnifeAttack knifeAttack;
    AttackTarget grenadeAttack;

    Cooldown grenadeAttackCooldown = new Cooldown(1f);
    Cooldown knifeCooldown = new Cooldown(2f);
    Cooldown walkBackCooldown = new Cooldown(3f);

    private Collider2D proximityCollider;
    private Collider2D midRangeCollider;

    protected override void Init() {
        proximityCollider = areaColliders[0];
        midRangeCollider = areaColliders[1];

        knifeAttack = GetComponent<EnemyKnifeAttack>();
        grenadeAttack = GetComponent<AttackTarget>();
    }

    protected override IEnumerator Think() {
        while (enabled) {

            if (!TargetInArea(midRangeCollider.bounds)) {
                yield return StartCoroutine( WalkTo(midRangeCollider) );
            } 

            while (TargetInArea( midRangeCollider.bounds ))
            {
                float randomValue = UnityEngine.Random.value;

                if (randomValue > 0.35 && grenadeAttackCooldown.IsReady()) {
                    yield return StartCoroutine( AttackGrenade() );
                } else if (walkBackCooldown.IsReady()) {
                    yield return StartCoroutine( WalkBackATinyBit() );
                } else {
                    yield return StartCoroutine( WalkTo(proximityCollider) );
                }

                yield return new WaitForSeconds(0.2f);
            }

            while (TargetInArea( proximityCollider.bounds ))
            {
                float randomValue = UnityEngine.Random.value;

                if (randomValue > 0.3f) {

                    yield return StartCoroutine( AttackKnife() );
                } else {
                    yield return StartCoroutine( WalkAway() );
                }

                yield return new WaitForSeconds(0.5f);
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator AttackKnife() {
        EnemyMovement.FaceTarget(transform, targetDistance.Target);
        knifeAttack.Execute("Player");

        yield return new WaitForSeconds(1.5f);
    }
    //TODO !! FACE AREA
    IEnumerator WalkTo(Collider2D area) {
        print("start waling to " + area);

        //FIXME here should have a FaceArea function rather than faceTarger!!
        while (!TargetInArea(area.bounds)) {
            EnemyMovement.FaceTarget(transform, targetDistance.Target);
            physic.MoveForward(0.7f);

            yield return new WaitForEndOfFrame();
        }

        print("finished walking to " + area);

        //FIXME - this is to leave time to the animation to finish
        //yield return new WaitForSeconds(0.5f);
    }

    IEnumerator WalkAway() {
        EnemyMovement.TurnBackToTarget(transform, targetDistance.Target);

        float startTime = Time.realtimeSinceStartup;

        while ( Time.realtimeSinceStartup < startTime + 2.5f) {
            physic.MoveForward(0.7f);
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator WalkBackATinyBit() {
        EnemyMovement.FaceTarget(transform, targetDistance.Target);
        float startTime = Time.realtimeSinceStartup;

        while (Time.realtimeSinceStartup < startTime + 24f/30f) {
            //TODO hmmm, moveForward but it goes backward with negative vel
            physic.MoveForward(-0.1f);
            yield return new WaitForEndOfFrame();
        }

        walkBackCooldown.Start();
    }

    IEnumerator AttackGrenade() {
        EnemyMovement.FaceTarget(transform, targetDistance.Target);
        grenadeAttack.Execute("Player");

        yield return new WaitForSeconds(1.5f);

        grenadeAttackCooldown.Start();
    }

}

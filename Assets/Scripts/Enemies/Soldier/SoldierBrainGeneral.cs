using System;
using System.Collections;
using UnityEngine;
using EnemyUtils;

public class SoldierBrainGeneral : EnemyBrain {

    private EnemyKnifeAttack knifeAttack;
    private AttackTarget grenadeAttack;

    private Cooldown grenadeAttackCooldown = new Cooldown(1f);
    private Cooldown knifeCooldown = new Cooldown(2f);
    private Cooldown walkBackCooldown = new Cooldown(3f);

    private Collider2D colliderProximity;
    private Collider2D colliderMidRange;

    protected override void Init() {
        colliderProximity = areaColliders[0];
        colliderMidRange = areaColliders[1];

        knifeAttack = GetComponent<EnemyKnifeAttack>();
        grenadeAttack = GetComponent<AttackTarget>();
    }

    protected override IEnumerator Think() {
        while (enabled) {

            while (TargetInArea( colliderMidRange.bounds ))
            {
                float randomValue = UnityEngine.Random.value;

                if (randomValue > 0.35f && grenadeAttackCooldown.IsReady()) {
                    yield return StartCoroutine( AttackGrenade() );
                } else if (walkBackCooldown.IsReady()) {
                    yield return StartCoroutine( WalkBackATinyBit() );
                } else {
                    yield return StartCoroutine( WalkTo( colliderProximity ) );
                }
                yield return 0;
            }

            while (TargetInArea( colliderProximity.bounds ))
            {
                float randomValue = UnityEngine.Random.value;

                if (randomValue > 0.3f) {
                    yield return StartCoroutine( AttackKnife() );
                } else {
                    yield return StartCoroutine( WalkAway() );
                }
                yield return 0;
            }

            if (!TargetInArea( colliderMidRange.bounds ) && !TargetInArea(colliderProximity.bounds)) {
                yield return StartCoroutine( WalkToPlayer() );
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator Test()
    {
        while (TargetInArea(colliderMidRange.bounds))
        {
            float randomValue = UnityEngine.Random.value;

            if (randomValue > 0.35f && grenadeAttackCooldown.IsReady())
            {
                yield return StartCoroutine(AttackGrenade());
            }
            else if (walkBackCooldown.IsReady())
            {
                yield return StartCoroutine(WalkBackATinyBit());
            }
            else
            {
                yield return StartCoroutine(WalkTo(colliderProximity));
            }
            yield return new WaitForSeconds(0.3f);
        }
    }

    IEnumerator AttackKnife() {
        EnemyMovement.FaceTarget(transform, targetDistance.Target);
        knifeAttack.Execute("Player");

        yield return new WaitForSeconds(1.5f);
    }

    IEnumerator WalkTo(Collider2D area) {
        const float walkMaxDuration = 3f;
        float startTime = Time.realtimeSinceStartup; 

        while (!TargetInArea(area.bounds) && Time.realtimeSinceStartup < startTime + walkMaxDuration) {
            EnemyMovement.FaceTarget(transform, targetDistance.Target);
            physic.MoveForward(0.7f);

            yield return new WaitForEndOfFrame();
        }

        //LEAVE TIME FOR SLOWING DOWN ANIM
        yield return new WaitForSeconds(0.5f);

    }

    bool TimeIsUp(float startTime, float duration) {
        return Time.realtimeSinceStartup > startTime + duration;
    }

    IEnumerator WalkToPlayer() {
        EnemyMovement.FaceTarget(transform, targetDistance.Target);
        float startTime = Time.realtimeSinceStartup;

        while (targetDistance.MoreThan(0.3f) && !TimeIsUp(startTime, 2f)) {
            physic.MoveForward(0.7f);
            yield return new WaitForEndOfFrame();
        }

        //LEAVE TIME FOR SLOWING DOWN ANIM
        yield return new WaitForSeconds(0.5f);
    }

    IEnumerator WalkAway() {
        EnemyMovement.TurnBackToTarget(transform, targetDistance.Target);
        float startTime = Time.realtimeSinceStartup;

        while (!TimeIsUp(startTime, 2f)) {
            physic.MoveForward(0.85f);
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator WalkBackATinyBit() {
        EnemyMovement.FaceTarget(transform, targetDistance.Target);
        float startTime = Time.realtimeSinceStartup;

        while (Time.realtimeSinceStartup < startTime + 24f/30f) {
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

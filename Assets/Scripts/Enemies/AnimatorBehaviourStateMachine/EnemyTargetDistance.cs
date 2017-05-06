using System;
using UnityEngine;

public class EnemyTargetDistance {
    private float targetDistance = 10;
    private float targetDistanceAbs = 10;
    public Transform Target { get; set; }
    public Transform Enemy { get; private set; }

    public EnemyTargetDistance(Transform enemyTransform, Transform targetTransform) {
        if (enemyTransform == null || targetTransform == null || enemyTransform == targetTransform) {
        //    throw new ArgumentException();
        } 
        Enemy = enemyTransform;
        Target = targetTransform;
    }

    public bool Between(float distMin, float distMax) {
        if (distMin > distMax) {
            throw new ArgumentException();
        }

        UpdateTargetDistance();
        return targetDistanceAbs > distMin && targetDistanceAbs < distMax;
    }

    public bool MoreThan(float dist) {
        UpdateTargetDistance();
        return targetDistanceAbs > dist;
    }

    public bool LessThan(float dist) {
        UpdateTargetDistance();
        return targetDistanceAbs < dist;
    }

    public float GetAbs() {
        UpdateTargetDistance();
        return targetDistanceAbs;
    }

    private void UpdateTargetDistance() {
        targetDistance = Enemy.position.x - Target.position.x;
        targetDistanceAbs = Mathf.Abs(targetDistance);
    }
}

using System;
using UnityEngine;

public class EnemyTargetDistance : MonoBehaviour {
    private float targetDistance = 10;
    private float targetDistanceAbs = 10;
    public Transform Target { get; set; }

    void Update() {
        if (Target == null) {
            Debug.LogError("No target to calculate Distance!!");
        } else {
            UpdateTargetDistance();
        }
    }

    public bool Between(float distMin, float distMax) {
        if (distMin > distMax) {
            throw new ArgumentException();
        }
        return targetDistanceAbs > distMin && targetDistanceAbs < distMax;
    }

    public bool MoreThan(float dist) {
        return targetDistanceAbs > dist;
    }

    public bool LessThan(float dist) {
        return targetDistanceAbs < dist;
    }

    public float GetAbs() {
        return targetDistanceAbs;
    }

    private void UpdateTargetDistance() {
        targetDistance = transform.position.x - Target.position.x;
        targetDistanceAbs = Mathf.Abs(targetDistance);
    }
}

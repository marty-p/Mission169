using UnityEngine;
using System.Collections;
using UnityEditor;

public abstract class EnemyBrain : MonoBehaviour {

    protected Transform target;
    [SerializeField]
    protected SlugPhysics physic;
    protected EnemyTargetDistance targetDistance;

    [SerializeField]
    protected Collider2D[] areaColliders;

    void Start() {
        AssignTarget();
        targetDistance = new EnemyTargetDistance(transform, target);

        Init();

        if (target != null) {
            StartCoroutine(Think());
        }
    }

    void Update() {
        if (target == null) {
            AssignTarget();
            targetDistance.Target = target;

            if (target != null) {
                StartCoroutine(Think());
            }
        }
    }

    protected abstract IEnumerator Think();

    public virtual void Pause() {
        StopAllCoroutines();
        enabled = false;
    }

    public virtual void Reset() {
        StopAllCoroutines();
        enabled = true;
    }

    public void OnDisable() {
        StopAllCoroutines();
    }

    public bool TargetInArea(Bounds bounds) {
        return bounds.max.x > target.position.x && bounds.min.x < target.position.x;
    }

    protected abstract void Init();

    protected void AssignTarget() {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

}

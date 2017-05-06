using UnityEngine;
using System.Collections;

public abstract class EnemyBrain : MonoBehaviour {

    protected Transform target;
    protected SlugPhysics physic;
    protected EnemyTargetDistance targetDistance;

    void Start() {
        AssignTarget();
        targetDistance = new EnemyTargetDistance(transform, target);

        physic = GetComponent<SlugPhysics>();

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

    protected abstract void Init();

    protected void AssignTarget() {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

}

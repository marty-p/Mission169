using UnityEngine;

public class BasicProjectile : MonoBehaviour, IProjectile {

    public ProjectileProperties properties;
    public Animator impactAnimator;
    private bool launched;

    public void OnTriggerEnter2D(Collider2D col) {
        if (col.tag == properties.victimTag || col.tag == "World") {
            ProjectileUtils.RandomizeImpactPosition(transform, impactAnimator.transform);
            impactAnimator.gameObject.SetActive(true);
            if (col.tag == "World") {
                impactAnimator.transform.right = transform.right;
                impactAnimator.Play("2");
            } else {
                impactAnimator.Play("1");
                ProjectileUtils.NotifyCollider(col, properties);
            }

            gameObject.SetActive(false);
        }
    }

    void OnBecameInvisible() {
        gameObject.SetActive(false);
        launched = false;
    }

    public void Launch(string victimsTag, Vector2 unusedDestination) {
        transform.localPosition = Vector3.zero;
        properties.victimTag = victimsTag;
        launched = true;
    }

    void Update() {
        if (launched) {
            ProjectileUtils.UpdatePositionStraightLine(transform, properties);
        }
    }

}

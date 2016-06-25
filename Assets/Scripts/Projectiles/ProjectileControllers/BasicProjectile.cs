using UnityEngine;

public class BasicProjectile : MonoBehaviour, IProjectile {

    public ProjectileProperties properties;
    private bool launched;

    public void OnTriggerEnter2D(Collider2D col) {
        if (col.tag == properties.victimTag || col.tag == "World") {
            ProjectileUtils.ImpactAnimation(transform, col, properties);
            ProjectileUtils.NotifyCollider(col, properties);
            gameObject.SetActive(false);
        }
    }

    void OnBecameInvisible() {
        gameObject.SetActive(false);
        launched = false;
    }

    public void Launch(string victimsTag, Vector2 unusedDestination) {
        properties.victimTag = victimsTag;
        launched = true;
    }

    void FixedUpdate() {
        if (launched) {
            ProjectileUtils.UpdatePositionStraightLine(transform, properties);
        }
    }

}

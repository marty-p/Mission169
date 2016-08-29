using UnityEngine;
using System.Collections;

public class SarubiaProjectile : MonoBehaviour, IProjectile {

    public ProjectileProperties properties;

    public void OnTriggerEnter2D(Collider2D col) {
        if (col.tag == properties.victimTag) {
            ProjectileUtils.ImpactAnimation(transform, col, properties);
            ProjectileUtils.NotifyCollider(col, properties);
            gameObject.SetActive(false);
        }
    }

    void OnBecameInvisible() {
        gameObject.SetActive(false);
    }

    public void Launch(string victimsTag, Vector2 unusedDestination) {
        properties.victimTag = victimsTag;
    }
}

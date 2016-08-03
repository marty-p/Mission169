using UnityEngine;

public class MummyBreath : MonoBehaviour {
    private Animator breathAnimator;
    private BoxCollider2D col;
    public ProjectileProperties properties;

	void Awake () {
        breathAnimator = GetComponent<Animator>();
        col = GetComponent<BoxCollider2D>();
	}

    public void ThrowBreath() {
        gameObject.SetActive(true);
        breathAnimator.SetTrigger("breath");
        //col.enabled = true;
    }

    public void OnEndOfExplosion() {
        gameObject.SetActive(false);
        //col.enabled = false;
    }

    public void OnTriggerEnter2D(Collider2D col) {
        if (col.tag == "Player") {
            if (properties.explosionAnimator != null) {
                ProjectileUtils.ImpactAnimation(transform, col, properties);
            }
            ProjectileUtils.NotifyCollider(col, properties);
            // NO WE DONT DO THAT FOR BREATH OR ANY AOE PROJ  gameObject.SetActive(false);
            // 2 types de proj se distingue les aoe (leurs effets dure sur la duree et sur un espace)
            // et les non aoe (bullets)
        }
    }
    


}

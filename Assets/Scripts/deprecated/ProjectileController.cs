using UnityEngine;
using System;
using Utils;



public class ProjectileController : MonoBehaviour {

    public ProjectileProperties projectileProperties;
    public RuntimeAnimatorController explosionAnimator;
    private bool projectileIsPhysic;
    private string victimTag = "undefined";

   
    void Awake() {
        projectileIsPhysic = GetComponent<SlugPhysics>() != null;
    }

    public virtual void OnTriggerEnter2D(Collider2D col) {
        if (col.tag == victimTag) {
            ImpactAnimation();
            NotifyVictim(col);
            gameObject.SetActive(false);
        }
    }

    public void NotifyVictim(Collider2D col) {
        HealthManager healthManager = col.GetComponentInChildren<HealthManager>();
        if ( healthManager != null) {
            healthManager.OnHitByProjectile(projectileProperties);
        }
    }

    public void ImpactAnimation() {
        Animator anim = SimpleAnimatorPool.GetPooledAnimator();
        anim.runtimeAnimatorController = explosionAnimator;
        anim.transform.position = (Vector2)transform.position + UnityEngine.Random.insideUnitCircle * 0.055f;
    }

    void Update() {
        if (projectileIsPhysic) {
            return;
        } else {
            UpdatePosition();
        }
    }

    void OnBecameInvisible() {
        gameObject.SetActive(false);
    }

    public void Throw(Vector3 direction, string vic) {
        victimTag = vic;
        transform.right = direction;
    }

    public void UpdatePosition() {
        transform.Translate(
                Vector3.right * projectileProperties.speedInUnityUnitPerSec * Time.deltaTime);
    }

}

using UnityEngine;
using System;

[Serializable]
public enum ProjectileType {
    Bullet,
    Knife,
    Grenade,
    Flame
}

[Serializable]
public class ProjectileProperties {
    public int strength = 1;
    public int speedInUnityUnitPerSec = 5;
    public ProjectileType type;
    public RuntimeAnimatorController explosionAnimator;
    [HideInInspector]
    public string victimTag = "undefined";
}


public class ProjectileUtils {

    public static void NotifyCollider(Collider2D col, ProjectileProperties projProp) {
        HealthManager healthManager = col.GetComponentInChildren<HealthManager>();
        if ( healthManager != null) {
            healthManager.OnHitByProjectile(projProp);
        } else {
            Debug.Log("Collider " + col.tag + " does not have a HealthManager");
        }
    }

    public static void UpdatePositionStraightLine(Transform proj, ProjectileProperties projProp) {
        proj.Translate(Vector3.right * projProp.speedInUnityUnitPerSec * Time.fixedDeltaTime);
    }

    public static void ImpactAnimation(Transform proj, Collider2D col, ProjectileProperties projProp) {
        Animator anim = SimpleAnimatorPool.GetPooledAnimator();
        anim.transform.right = Vector2.right;
        anim.runtimeAnimatorController = projProp.explosionAnimator;
        anim.transform.position = (Vector2) proj.transform.position + UnityEngine.Random.insideUnitCircle * 0.05f;
        anim.Play("1");
    }

    public static Animator GetImpactAnimator(Transform proj, ProjectileProperties projProp) {
        Animator anim = SimpleAnimatorPool.GetPooledAnimator();
        anim.transform.right = Vector2.right;
        anim.runtimeAnimatorController = projProp.explosionAnimator;
        anim.transform.position = (Vector2) proj.transform.position + UnityEngine.Random.insideUnitCircle * 0.05f;
        return anim;
    }

}

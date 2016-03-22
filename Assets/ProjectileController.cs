using UnityEngine;
using System;


[Serializable]
public enum BulletType {
    Bullet,
    Knife,
    Grenade,
    Flame
}

[Serializable]
public class BulletProperties {
    public int strength = 1;
    public int speedInUnityUnitPerSec = 5;
    public BulletType bulletType;
}

public class ProjectileController : MonoBehaviour {

    public BulletProperties bulletProperties;
    public RuntimeAnimatorController explosionAnimator;
    private bool projectileIsPhysic;

    // Works for something that is fully symetric of course
    private Vector3 rightOrientation = new Vector3(0, 0, 0);
    private Vector3 leftOrientation = new Vector3(0, 0, 180);
    private Vector3 upOrientation = new Vector3(0, 0, 90);
    private Vector3 downOrientation = new Vector3(0, 0, -90);
   
    void Awake() {
        projectileIsPhysic = GetComponent<PhysicsSlugEngine>() != null;
    }

    public virtual void OnTriggerEnter2D(Collider2D col) {
        if (col.tag == "enemy") {
            Impact();
            IHitByProjectile component = col.GetComponentInChildren<IHitByProjectile>();
            component.OnHitByProjectile(bulletProperties.strength, bulletProperties.bulletType,
                    (int)transform.right.x);
            gameObject.SetActive(false);
        }
    }

    public void Impact() {
        Animator anim = SimpleAnimatorPool.GetPooledAnimator();
        anim.runtimeAnimatorController = explosionAnimator;
        anim.transform.position = transform.position;
    }

    void FixedUpdate() {
        if (projectileIsPhysic) {
            return;
        } else {
            UpdatePosition();
        }
    }

    void OnBecameInvisible() {
        gameObject.SetActive(false);
    }

    public void Throw(Vector3 direction) {
        SetOrientation(direction);
    }

    public void SetOrientation(Vector3 dir) {
        if (dir == Vector3.right) {
            transform.eulerAngles = rightOrientation;
        } else if (dir == Vector3.left) {
            transform.eulerAngles = leftOrientation;
        } else if (dir == Vector3.up) {
            transform.eulerAngles = upOrientation;
        } else if (dir == Vector3.down) {
            transform.eulerAngles = downOrientation;
        }
    }

    void UpdatePosition() {
        transform.Translate(
                Vector3.right * bulletProperties.speedInUnityUnitPerSec * Time.fixedDeltaTime);
    }

}

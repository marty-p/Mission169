using UnityEngine;

public class HealthManager : MonoBehaviour, IHitByProjectile, IObserver {

    public int maxHP = 1;
    private int currentHP = 1;

    private Animator anim;
    private EnemyBehaviorManager enemyBehaviorManager;
    private PhysicsSlugEngine physics;

    public void OnHitByProjectile(int damageReceived, BulletType bulletType, int projectileDirX) {
        currentHP -= damageReceived;
        PickVisualFeedback(projectileDirX, bulletType);
    }

    public void PickVisualFeedback(int projectileDirX, BulletType bulletType) {
        if (currentHP < 1) {
            if (bulletType == BulletType.Bullet) {
                HitByBullet(projectileDirX);
            } else if (bulletType == BulletType.Grenade) {
                HitByGrenade();
            } else if (bulletType == BulletType.Knife) {
                Slashed();
            }
        } else {
            // Flash redscale for a frame.
        }
    }

    public void Slashed() {
        anim.SetTrigger("slashed");
        Death();
    }

    public void HitByGrenade() {
        anim.SetBool("hit_by_grenade", true);
        physics.SetVelocity(1);
        Death();
    }

    public void HitByBullet(int projectileDirX) {
        if (projectileDirX == transform.right.x) {
            anim.SetTrigger("hit_by_bullet2");
        } else {
            anim.SetTrigger("hit_by_bullet");
        }
        Death();
    }


    void Death() {
        enemyBehaviorManager.enabled = false;
    }

    void Start () {
        anim = GetComponent<Animator>();
        enemyBehaviorManager = GetComponent<EnemyBehaviorManager>();
        physics = GetComponent<PhysicsSlugEngine>();
	}

    public void Observe(SlugEvents ev) {
        if (ev == SlugEvents.HitGround) {
            if (anim.GetBool("hit_by_grenade")) {
                anim.SetBool("hit_by_grenade", false);
                anim.SetTrigger("hit_ground");
            }
        }
    }
}

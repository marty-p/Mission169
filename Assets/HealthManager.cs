using UnityEngine;
using System.Collections;
using System;


public class HealthManager : MonoBehaviour, IHitByProjectile, IObserver {

    public int maxHP = 1;
    private int currentHP = 1;

    private Animator anim;
    private EnemyBehaviorManager enemyBehaviorManager;
    private PhysicsSlugEngine physics;

    public void OnHitByProjectile(int damageReceived, int projectileDirX) {
        currentHP -= damageReceived;
        PickVisualFeedback(projectileDirX);
    }

    public void HitByProjectile () {
        // combien de degats?
        // suis-je mort?
        PickVisualFeedback(1);
    }

    public void OnSlashed() {
        anim.SetTrigger("slashed");
        Death();
    }

    public void OnHitByGrenade() {
        anim.SetBool("hit_by_grenade", true);
        physics.Jump();
        Death();
    }

    public void PickVisualFeedback(int projectileDirX) {
        if (currentHP < 1) {
            if (projectileDirX == transform.right.x) {
                anim.SetTrigger("hit_by_bullet2");
            } else {
                anim.SetTrigger("hit_by_bullet");
            }
            Death();
        } else {
            // Flash redscale for a frame.
        }
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

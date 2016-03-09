using UnityEngine;
using System.Collections;
using System;

public class HealthManager : MonoBehaviour, IHitByProjectile {

    public int maxHP = 1;
    private int currentHP = 1;

    private Animator anim;
    private Rigidbody2D rigidBody;
    private EnemyBehaviorManager enemyBehaviorManager;

    public void OnHitByProjectile(int damageReceived, int projectileDirX) {
        currentHP -= damageReceived;
        PickVisualFeedback(projectileDirX);
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
        rigidBody = GetComponent<Rigidbody2D>();
        enemyBehaviorManager = GetComponent<EnemyBehaviorManager>();
	}

}

using UnityEngine;
using System.Collections;
using System;

public class AttackManager : MonoBehaviour, IObserver {

    public Animator animator;
    public ObjectPoolScript objectPool;
    public Transform bulletInitialPosition;
    private bool locked;
    public BoxCollider2D boxCollider;
    private bool isLookingUp = false;

    private bool contactWithEnemy = false;

    public void Observe(SlugEvents ev) {
        if (ev == SlugEvents.Attack) {
            if (contactWithEnemy) {
                KnifeAttack();
            } else {
                if (isLookingUp) {
                    GunAttack(transform.up);
                } else {
                    GunAttack(transform.right);
                }
            }
        } else if (ev == SlugEvents.LookUp) {
            isLookingUp = true;
        } else if (ev == SlugEvents.Stand) {
            isLookingUp = false;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "enemy") {
            contactWithEnemy = true;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "enemy") {
            contactWithEnemy = false;
        }
    }

    private void KnifeAttack() {
        animator.SetTrigger("knife");
    }

   private void GunAttack(Vector3 dir) {
        animator.SetTrigger("fire");
        GameObject bulletGameObject = objectPool.GetPooledObject();
        BulletController bullet  = bulletGameObject.GetComponent<BulletController>();
        bullet.InitPosition(bulletInitialPosition.position);
        bullet.Fire(dir);
    }

}

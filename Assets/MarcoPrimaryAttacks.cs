using UnityEngine;
using System.Collections;
using System;

public class MarcoPrimaryAttacks : MonoBehaviour, IObserver {

    public Animator anim;
    private bool inRangeForKnife;

    public ObjectPoolScript bullettPool;
    private bool lookingUp;
    private bool sitting;
    private bool walking;
    public Transform bulletInitialPosition;
    public Transform bulletInitialPositionSitting;
    public Transform bulletInitialPositionWalking;

    public void Observe(SlugEvents ev) {
        if (ev == SlugEvents.Attack) {
            if (inRangeForKnife) {
                KnifeAttack();
            } else {
                if (lookingUp) {
                    GunAttack(transform.up, bulletInitialPosition);
                } else if (sitting) {
                    GunAttack(transform.right, bulletInitialPositionSitting);
                } else if (walking) {
                    GunAttack(transform.right, bulletInitialPositionWalking);
                } else {
                    GunAttack(transform.right, bulletInitialPosition);
                }
            }
        } else if (ev == SlugEvents.LookUp) {
            lookingUp = true;
        } else if (ev == SlugEvents.Stand) {
            lookingUp = false;
            sitting = false;
            walking = false;
        } else if (ev == SlugEvents.Sit) {
            sitting = true;
        } else if (ev == SlugEvents.MovingLeft || ev == SlugEvents.MovingRight) {
            walking = true;
        }
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (col.tag == "enemy") {
            inRangeForKnife = true;
        }
    }

    void OnTriggerExit2D(Collider2D col) {
        if (col.tag == "enemy") {
            inRangeForKnife = false;
        }
    }

    public void KnifeAttack() {
        anim.SetTrigger("knife");
        Vector2 origin = new Vector2(transform.position.x, transform.position.y);
        Vector2 direction = transform.right;
        float distance = 0.25f;
        RaycastHit2D[] hits = Physics2D.RaycastAll(origin, direction, distance);
        foreach(RaycastHit2D hit in hits) {
            if (hit.collider.tag == "enemy") {
                IHitByProjectile enemy = hit.collider.GetComponent<IHitByProjectile>();
                enemy.OnHitByProjectile(1, BulletType.Knife, (int)transform.right.x);
            }
        }
    }

    public void GunAttack(Vector3 dir, Transform bulletPosition) {
        anim.SetTrigger("fire");
        GameObject bulletGameObject = bullettPool.GetPooledObject();
        ProjectileController bullet = bulletGameObject.GetComponent<ProjectileController>();
        bullet.transform.position = bulletPosition.position;
        bullet.Throw(dir);
    }
}

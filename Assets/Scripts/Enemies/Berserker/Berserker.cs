using System;
using UnityEngine;

public class Berserker : MonoBehaviour, IReceiveDamage {

    public GameObject topBody;
    public GameObject BottomBody;
    public Animator BottomBodyAnimator;
    private SlugPhysics physic;
    private AreaOfEffectProjectile knife;

    void Awake() {
        physic = GetComponent<SlugPhysics>();
        knife = GetComponent<AreaOfEffectProjectile>();
    }

    public void OnEnable() {
    }

    public void FixedUpdate() {
        physic.MoveForward();
    }

    public void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player") {
            Attack();
        }
    }

    private void Attack() {
        topBody.SetActive(true);
        BottomBodyAnimator.SetBool("attack", true);
        knife.CastAOE("Player", transform.position); 
    }

    public void OnDamageReceived(ProjectileProperties projectileProp, int newHP) {
        if (newHP < 1) {
            physic.enabled = false;
            topBody.SetActive(false);
            BottomBody.SetActive(false);
        }
    }


}

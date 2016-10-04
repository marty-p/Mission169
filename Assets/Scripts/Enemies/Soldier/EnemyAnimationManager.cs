using System;
using UnityEngine;
using Utils;

public class EnemyAnimationManager : MonoBehaviour, IObserver {

    private Animator anim;
    public Animator blood;
    private RetVoidTakeVoid grenadeCB;
    private RetVoidTakeVoid grenadeEndCB;
    private string[] deathTriggers;
    private BoxCollider2D boxCollider;
    private TimeUtils timeUtils;
    private float minHeightToFall = -0.2f;

    void Awake() {
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        timeUtils = GetComponent<TimeUtils>();
    }

    void Start () {
        deathTriggers = new String[3];
        deathTriggers[0] = "hit_by_bullet1";
        //deathTriggers[1] = "hit_by_bullet2";
        deathTriggers[1] = "hit_by_bullet3";
        deathTriggers[2] = "hit_by_bullet4";
	}

    public void Observe(SlugEvents ev) {
        if (ev == SlugEvents.HitGround) {
            if (anim.GetBool("hit_by_grenade")) {
                anim.SetBool("hit_by_grenade", false);
            } else if(anim.GetBool("fall_death")) {
                anim.SetBool("fall_death", false);
            }
        }
    }

    public void PlayDeathAnimation(ProjectileProperties proj) {
        ProjectileType bulletType = proj.type;
        if (bulletType == ProjectileType.Bullet) {
            //TODO raycast to see if there is other world collider beneath
            if (transform.position.y > minHeightToFall) {
                anim.SetBool("fall_death", true);
            } else {
                string deathTrigger = GetRandomDeathTrigger();
                anim.SetTrigger(deathTrigger);
            }

            blood.Play("1");
        } else if (bulletType == ProjectileType.Grenade) {
            anim.SetBool("hit_by_grenade", true);
        } else if (bulletType == ProjectileType.Knife) {
            anim.SetTrigger("slashed");
        }
    }

    public void AEStartDeathFall() {
        boxCollider.enabled = false;
        timeUtils.TimeDelay( 0.2f,() => { boxCollider.enabled = true;});
    }

    private string GetRandomDeathTrigger() {
        float randFloat = UnityEngine.Random.value * deathTriggers.Length;
        int randInt = (int)randFloat;
        return deathTriggers[randInt];
    }

}

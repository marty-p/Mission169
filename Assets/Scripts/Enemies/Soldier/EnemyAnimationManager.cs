using System;
using UnityEngine;
using Utils;

public class EnemyAnimationManager : MonoBehaviour, IObserver {

    private Animator anim;
    public Animator blood;
    private RetVoidTakeVoid grenadeCB;
    private RetVoidTakeVoid grenadeEndCB;
    private string[] deathTriggers;  

    void Start () {
        anim = GetComponent<Animator>();

        deathTriggers = new String[3];
        deathTriggers[0] = "hit_by_bullet1";
      //  deathTriggers[1] = "hit_by_bullet2";
        deathTriggers[1] = "hit_by_bullet3";
        deathTriggers[2] = "hit_by_bullet4";
	}

    public void Observe(SlugEvents ev) {
        if (ev == SlugEvents.HitGround) {
            if (anim.GetBool("hit_by_grenade")) {
                anim.SetBool("hit_by_grenade", false);
            }
        }
    }

    public void PlayDeathAnimation(ProjectileProperties proj) {
        ProjectileType bulletType = proj.type;
        if (bulletType == ProjectileType.Bullet) {
            string deathTrigger = GetRandomDeathTrigger();
            anim.SetTrigger(deathTrigger);
            blood.Play("1");
        } else if (bulletType == ProjectileType.Grenade) {
            anim.SetBool("hit_by_grenade", true);
        } else if (bulletType == ProjectileType.Knife) {
            anim.SetTrigger("slashed");
        }
    }

    private string GetRandomDeathTrigger() {
        float randFloat = UnityEngine.Random.value * deathTriggers.Length;
        int randInt = (int)randFloat;
        return deathTriggers[randInt];
    }

}

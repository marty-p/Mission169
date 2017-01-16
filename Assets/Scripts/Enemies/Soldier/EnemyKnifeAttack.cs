using System;
using UnityEngine;

public class EnemyKnifeAttack : MonoBehaviour, IAttack {

    private Animator anim;
    private string victimsTag = "Player";
    public AreaOfEffectProjectile knife;
    public bool attacking;

    public void Awake() {
        anim = GetComponent<Animator>();
    }

    public void Execute(string victimsTag, Vector3 dirUnused = default(Vector3), Vector3 ProjectileInitalPosUnused = default(Vector3)) {
        anim.SetTrigger("knife");
        this.victimsTag = victimsTag;
        attacking = true;
    }

    public void AEKnifeIsOut() {
        knife.CastAOE(victimsTag, transform.position);
    }

    public void AEKnifeIsDone() {
        attacking = false;
    }

}

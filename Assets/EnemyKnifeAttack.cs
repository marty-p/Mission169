using System;
using UnityEngine;

public class EnemyKnifeAttack : MonoBehaviour, IAttack {

    private Animator anim;
    private string victimsTag = "Player";
    public AreaOfEffectProjectile knife;
    public float coolDown = 2;
    private float lastAttackTimeStamp;


    public void Awake() {
        anim = GetComponent<Animator>();
    }

    public void Execute(string victimsTag, Vector3 dirUnused, Vector3 ProjectileInitalPosUnused) {
        anim.SetTrigger("knife");
        this.victimsTag = victimsTag;
    }

    public void OnKnifeIsOut() {
        knife.CastAOE(victimsTag, transform.position);
        lastAttackTimeStamp = Time.time;
    }

    public bool IsReady() {
        return (Time.time - lastAttackTimeStamp) > coolDown;
    }

}

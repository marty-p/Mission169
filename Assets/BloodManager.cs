using UnityEngine;
using System.Collections;
using System;

public class BloodManager : MonoBehaviour, IHitByProjectile {

    private Animator anim;

    void Start () {
        anim = GetComponent<Animator>();
        anim.enabled = false;
	}

    public void StartBloodAnim() {
        anim.enabled = true;
        //anim.SetTrigger("blood1");
    }

    public void OnHitByProjectile(int damageReceived, BulletType bulletType, int bulletDirX) {
        StartBloodAnim();
    }
}

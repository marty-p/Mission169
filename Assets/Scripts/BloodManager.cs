using UnityEngine;
using System.Collections;
using System;

public class BloodManager : MonoBehaviour, IReceiveDamage {

    private Animator anim;

    void Start () {
        anim = GetComponent<Animator>();
	}

    public void StartBloodAnim() {
        anim.SetTrigger("blood1");
    }
 
    public void OnDamageReceived(ProjectileProperties projectileProp, int newHP) {
        StartBloodAnim();
    }
}

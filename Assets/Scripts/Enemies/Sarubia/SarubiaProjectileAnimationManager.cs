using UnityEngine;
using System;

public class SarubiaProjectileAnimationManager : MonoBehaviour, IObserver {

    private Animator anim;

    void Start () {
        anim = GetComponent<Animator>();
	}

    public void Observe(SlugEvents ev) {
        if (ev == SlugEvents.HitGround) {
            anim.SetTrigger("bounce");
        }
    }

	
}

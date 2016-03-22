using UnityEngine;
using System.Collections;
using System;

public class AttackGrenade : MonoBehaviour, IObserver {

    public ObjectPoolScript grenadePool;
    public Animator animator;
    public Transform initialPosition;

    public void Observe(SlugEvents ev) {
        if (ev == SlugEvents.Grenade) {
            GrenadeAttack();
        }
    }

    private void GrenadeAttack() {
        animator.SetTrigger("grenade");
        GameObject grenadeGameObject = grenadePool.GetPooledObject();
        GrenadeController grenade  = grenadeGameObject.GetComponent<GrenadeController>();
        grenade.transform.position = initialPosition.position;
        grenade.Init();
        grenade.Throw(transform.right);
    }	

}

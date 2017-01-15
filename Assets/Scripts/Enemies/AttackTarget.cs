using System;
using UnityEngine;

public class AttackTarget : MonoBehaviour, IAttack {

    public GameObject grenade;
    private GrenadeParabola grenadeParabola;
    public string victimsTag = "Player";
    private Animator animator;
    private Transform target;
    public bool attacking;

    void Awake () {
        animator = GetComponent<Animator>();
	}

    void Start() {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        grenadeParabola = grenade.GetComponentInChildren<GrenadeParabola>(true);
    }

    public void Execute(string victimTag, Vector3 dir = default(Vector3), Vector3 ProjectileInitalPos = default(Vector3)) {
        animator.SetTrigger("grenade_standing");
        attacking = true;
    }

    public void AEGrenadeAttack() {
        Vector2 projectileDestination = target.transform.position;
        grenade.SetActive(false);
        grenade.SetActive(true);
        grenade.transform.position = transform.position + new Vector3(0, 0.2f);
        grenade.transform.right = transform.right;
        grenadeParabola.Launch(victimsTag, projectileDestination);
    }

    public void AEGrenadeAttackDone() {
        attacking = false;
    }
}

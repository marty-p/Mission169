using System;
using UnityEngine;

public class AttackTarget : MonoBehaviour, IAttack {

    public GrenadeParabola grenade;
    public string victimsTag = "Player";
    private Animator animator;
    private Transform target;

    void Awake () {
        animator = GetComponent<Animator>();
	}

    void Start() {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void Execute(string victimTag, Vector3 dir = default(Vector3), Vector3 ProjectileInitalPos = default(Vector3)) {
        animator.SetTrigger("grenade_standing");
    }

    public void GrenadeAttack() {
        Vector2 projectileDestination = target.transform.position;
        grenade.transform.position = transform.position + new Vector3(0, 0.2f);
        grenade.transform.right = transform.right;
        grenade.gameObject.SetActive(true);
        grenade.Launch(victimsTag, projectileDestination);
    }
}

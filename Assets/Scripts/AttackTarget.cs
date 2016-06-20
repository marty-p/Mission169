using System;
using UnityEngine;

public class AttackTarget : MonoBehaviour, IAttack {

    public ObjectPoolScript grenadePool;
    public string victimsTag = "Player";
    private Animator animator;
    private bool inProgress = false;
    private Transform target;
    private float lastAttackTimeStamp;
    public float CoolDown = 3;

    void Awake () {
        animator = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
	}

    public void Execute(string victimTag, Vector3 dir = default(Vector3), Vector3 ProjectileInitalPos = default(Vector3)) {
        animator.SetTrigger("grenade_standing");
    }

    public void GrenadeAttack() {
        Vector2 projectileDestination = target.transform.position;
        GameObject grenadeGameObject = grenadePool.GetPooledObject();
        IProjectile grenade = grenadeGameObject.GetComponent<IProjectile>();
        grenadeGameObject.transform.position = transform.position + new Vector3(0,0.2f);
        grenadeGameObject.transform.right = transform.right;
        grenade.Launch(victimsTag, projectileDestination);
        lastAttackTimeStamp = Time.time;
    }

    public bool IsReady()
    {
        return (Time.time - lastAttackTimeStamp) > CoolDown;
    }
}

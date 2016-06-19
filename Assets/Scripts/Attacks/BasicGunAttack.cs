using UnityEngine;
using System;

public class BasicGunAttack : MonoBehaviour, IAttack {

    public Animator anim;
    public ObjectPoolScript bullettPool;

    public bool AttackInProgress() {
        throw new NotImplementedException();
    }

    public void Execute(string victimTag, Vector3 dir , Vector3 ProjectileInitalPos) {
        anim.SetTrigger("fire");

        GameObject bulletGameObject = bullettPool.GetPooledObject();
        bulletGameObject.transform.position = ProjectileInitalPos;
        bulletGameObject.transform.right = dir;

        IProjectile bullet = bulletGameObject.GetComponent<IProjectile>();
        bullet.Launch(victimTag);
    }

    public bool IsReady()
    {
        return true;
    }
}

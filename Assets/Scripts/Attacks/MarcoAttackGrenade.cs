using System;
using UnityEngine;

public class MarcoAttackGrenade : MonoBehaviour, IAttack {

    public ObjectPoolScript projectilePool;
    public AnimationManager animManager;
    private Vector3 initialPosition;
    public string victimTag = "untagged";
    private bool inProgress;

    public void Execute(string victimTag, Vector3 dirUnused, Vector3 projInitialPos) {
        animManager.StartGrenadeAnim(ThrowItAway);
        initialPosition = projInitialPos;
        this.victimTag = victimTag;
    }

    private void ThrowItAway () {
        GameObject grenadeGameObject = projectilePool.GetPooledObject();
        IProjectile grenade = grenadeGameObject.GetComponent<IProjectile>();
        grenadeGameObject.transform.position = initialPosition;
        grenadeGameObject.transform.right = transform.right;
        grenade.Launch(this.victimTag);
    }


    public bool IsReady()
    {
        return true;
    }
}

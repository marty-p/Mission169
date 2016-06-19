using System;
using UnityEngine;

public class AttackKnife : MonoBehaviour, IAttack {

    public Animator anim;
    public AreaOfEffectProjectile knife;
    private bool inProgress;

    public void Execute(string victimTag, Vector3 unused, Vector3 unused2) {
        anim.SetTrigger("knife");
        knife.CastAOE(victimTag, transform.position);
        inProgress = true;
    }

    public void OnKnifeAnimationIsOver() {
        inProgress = false;
    }
    
    public bool AttackInProgress() {
        return inProgress;
    }

    public bool IsReady()
    {
        return true;

    }
}

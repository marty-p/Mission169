using UnityEngine;

// Components that need to know about damages taken can't just
// implements IHitByProjectile because the projectile calls
// GetComponentsInChildren then order they're called depends on
// the order the components are in the inspector. I want to be
// sure the first components to be called is the HealthManager

public class HealthManager : MonoBehaviour {

    public int maxHP = 1;
    public int currentHP;
    public int CurrentHP {get {return currentHP;}}
    private IReceiveDamage[] componsInterestedInDamages;
    private bool ignoreDamages;
    public bool IgnoreDamages {get {return ignoreDamages;} set {ignoreDamages = value;}}

    void Start () {
        componsInterestedInDamages = GetComponentsInChildren<IReceiveDamage>();
        currentHP = maxHP;
	}

    void OnEnable() {
        currentHP = maxHP;
    }

    public void OnHitByProjectile(ProjectileProperties projectile) {
        if (ignoreDamages) {
            return;
        } else {
            currentHP -= projectile.strength;
            NotifyDamageWasTaken(projectile);
        }
    }

    private void NotifyDamageWasTaken(ProjectileProperties proj) {
        foreach (IReceiveDamage d in componsInterestedInDamages) {
            d.OnDamageReceived(proj, currentHP);
        }
    }
}

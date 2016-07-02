using UnityEngine;

public class DeathManager : MonoBehaviour, IReceiveDamage {

    private EnemyAnimationManager animManager;
    private SlugPhysics physics;

    void Start() {
        animManager = GetComponent<EnemyAnimationManager>();
        physics = GetComponent<SlugPhysics>();
    }

    public void OnDamageReceived(ProjectileProperties projectileProp, int newHP) {
        if (newHP > 0) {
            return;
        } else {
            Die(projectileProp);
        }
    }

    private void Die(ProjectileProperties projectile) {
        // to ignore collision with projectile
        gameObject.layer = 2; //TODO have an enum with the layer
        animManager.PlayDeathAnimation(projectile);
        if (projectile.type == ProjectileType.Grenade) {
            physics.SetVelocity(0.2f, 3.5f);
        }
    }

}

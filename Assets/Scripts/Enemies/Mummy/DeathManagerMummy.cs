using UnityEngine;
using Slug;

public class DeathManagerMummy : MonoBehaviour, IReceiveDamage {

    private FlashUsingMaterial flashRed;
    private Animator animator;

	void Start () {
        flashRed = GetComponent<FlashUsingMaterial>();
        animator = GetComponent<Animator>();
	}

    public void OnDamageReceived(ProjectileProperties projectileProp, int newHP) {
        if (newHP > 0) {
            flashRed.FlashOnce();
        } else {
            Die(projectileProp);
        }
    }

    private void Die(ProjectileProperties projProp) {
        animator.SetTrigger("death");
        gameObject.layer =(int) SlugLayers.Default;

    }
}

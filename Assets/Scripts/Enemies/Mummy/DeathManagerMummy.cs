using UnityEngine;
using Slug;

public class DeathManagerMummy : MonoBehaviour, IReceiveDamage {

    private FlashUsingMaterial flashRed;
    private Animator animator;
    private SlugAudioManager audioManager;

	void Awake () {
        flashRed = GetComponent<FlashUsingMaterial>();
        animator = GetComponent<Animator>();
        audioManager = GetComponent<SlugAudioManager>();
	}

    public void OnDamageReceived(ProjectileProperties projectileProp, int newHP) {
        if (newHP > 0) {
            flashRed.FlashSlugStyle();
        } else {
            Die(projectileProp);
        }
    }

    private void Die(ProjectileProperties projProp) {
        animator.SetTrigger("death");
        gameObject.layer =(int) SlugLayers.IgnoreRaycast;
        audioManager.PlaySound(0);
    }
}

using UnityEngine;
using System.Collections;

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
      //  transform.gameObject.SetActive(false);
      //  transform.localPosition = Vector3.zero;
    }
}

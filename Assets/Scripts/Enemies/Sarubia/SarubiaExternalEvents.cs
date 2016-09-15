using UnityEngine;
using Slug;

public class SarubiaExternalEvents : MonoBehaviour, IReceiveDamage {

    public FlashUsingMaterial flashRed;
    private Animator animator;
    private TimeUtils timeUtils;
    public RuntimeAnimatorController explosion;
    private BoxCollider2D collider;
    private bool dead;
    private SlugAudioManager audioManager;

    public void OnDamageReceived(ProjectileProperties projectileProp, int newHP) {
        if (newHP > 0) {
            flashRed.FlashSlugStyle();
            audioManager.PlaySound(0);
            return;
        } else if (!dead) {
            dead = true;
            gameObject.tag = "World";
            animator.SetTrigger("explode");
            float startTime = Time.time;
            float duration = 0.6f;
            Bounds boundsForExplosions = collider.bounds;
            timeUtils.RepeatEvery(0.085f, () => {
                // Visual 
                Animator anim = SimpleAnimatorPool.GetPooledAnimator(explosion);
                anim.transform.position = RandomPosWithin(boundsForExplosions);
                anim.Play("1");
                // Audio
                audioManager.PlaySound(1);
                if (Time.time > startTime + duration) {
                    return false;
                } else {
                    return true;
                }
            });
        }
    }

    Vector2 RandomPosWithin(Bounds bounds) {
        return new Vector2(
                bounds.min.x + UnityEngine.Random.Range(0, bounds.size.x),
                bounds.min.y + UnityEngine.Random.Range(0, bounds.size.y)
        );
    }

	void Update () {
        bool b = Input.GetKeyDown("b");
        if (b) {GetComponent<SarubiaAttackManager>().PrimaryAttack();}
	}

    void Awake() {
        animator = GetComponent<Animator>();
        timeUtils = GetComponent<TimeUtils>();
        collider = GetComponent<BoxCollider2D>();
        audioManager = GetComponentInChildren<SlugAudioManager>();
    }
}

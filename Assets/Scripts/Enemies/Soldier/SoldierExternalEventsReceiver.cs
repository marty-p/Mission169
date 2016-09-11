using UnityEngine;
using Slug;

public class SoldierExternalEventsReceiver : MonoBehaviour, IReceiveDamage {

    private Animator anim;
    private EnemyAnimationManager animManager;
    public float getScaredFactor = 0.5f;
    private Blink blink;
    private TimeUtils timeUtils;

    void Awake() {
        anim = GetComponent<Animator>();
        animManager = GetComponent<EnemyAnimationManager>();
        blink = GetComponent<Blink>();
        timeUtils = GetComponent<TimeUtils>();

        EventManager.StartListening("player_death", () => {
            anim.SetTrigger("laugh");
        });
        EventManager.StartListening("player_back_alive", () => {
            anim.Play("enemy_still");
            if (UnityEngine.Random.value < getScaredFactor) {
                anim.SetTrigger("scared");
                if (UnityEngine.Random.value < getScaredFactor) {
                    anim.SetTrigger("run_away");
                }
            }
        });
    }

    public void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player") {
            anim.SetTrigger("knife");
        }
    }

    public void OnDamageReceived(ProjectileProperties projectileProp, int newHP) {
        if (newHP > 0) {
            return;
        } else {
            //To ignore collision with projectiles during death anim but still be 'physic'
            gameObject.layer = 2;
            animManager.PlayDeathAnimation(projectileProp);
            EventManager.TriggerEvent("add_points", 100);
        }
    }

    // Animation Event
    public void OnEndOfDeathAnim() {
        timeUtils.TimeDelay(0.2f, () => {
            blink.BlinkPlease(() => {
                Transform t = transform;
                while (t.parent != null && t.parent.tag == "enemy") {
                    t = t.parent;
                }
                t.gameObject.SetActive(false);
            });
        });
    }

}

using UnityEngine;
using SlugLib;

public class SoldierExternalEventsReceiver : MonoBehaviour, IReceiveDamage {

    private Animator anim;
    private EnemyAnimationManager animManager;
    public float getScaredFactor = 0.5f;
    private Blink blink;
    private TimeUtils timeUtils;


    void Awake() {
        EventManager.Instance.StartListening(GlobalEvents.PlayerDead, () => {
            if (anim.isInitialized) {
                anim.SetTrigger("laugh");
            }
        });
        EventManager.Instance.StartListening(GlobalEvents.PlayerSpawned, () => {
            if (anim.isInitialized) {
                anim.Play("enemy_still");
                if (UnityEngine.Random.value < getScaredFactor) {
                    anim.SetTrigger("scared");
                    if (UnityEngine.Random.value < getScaredFactor) {
                        anim.SetTrigger("run_away");
                    }
                }
            }
        });
        EventManager.Instance.StartListening(GlobalEvents.MissionSuccess, () => {
            if (anim.isInitialized) {
                anim.SetTrigger("laugh");
            }
        });
    }

    void Start() {
        anim = GetComponent<Animator>();
        animManager = GetComponent<EnemyAnimationManager>();
        blink = GetComponent<Blink>();
        timeUtils = GetComponent<TimeUtils>();
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
            animManager.PlayDeathAnimation(projectileProp);
        }
    }

    // Animation Event
    public void OnEndOfDeathAnim() {
        timeUtils.TimeDelay(0.25f, () => {
            blink.BlinkPlease(() => {
                // FIXME - getting to the root Game object to set it inactive
                Transform t = transform;
                while (t.parent != null && t.parent.tag == "enemy") {
                    t = t.parent;
                }
                t.gameObject.SetActive(false);
            });
        });
    }

}

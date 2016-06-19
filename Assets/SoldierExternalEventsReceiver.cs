using UnityEngine;

public class SoldierExternalEventsReceiver : MonoBehaviour, IReceiveDamage {

    private Animator anim;
    private EnemyAnimationManager animManager;
    public float getScaredFactor = 0.5f;

    void Awake() {
        anim = GetComponent<Animator>();
        animManager = GetComponent<EnemyAnimationManager>();

        EventManager.StartListening("player_death", () => {
            anim.SetTrigger("laugh");
        });
        EventManager.StartListening("player_back_alive", () => {
            anim.Play("enemy_still");
            if (UnityEngine.Random.value < getScaredFactor) {
                anim.SetTrigger("scared");
            }
        });
    }

    public void OnDamageReceived(ProjectileProperties projectileProp, int newHP) {
        if (newHP > 0) {
            return;
        } else {
            //To ignore collision with projectiles during death anim
            gameObject.layer = 2; //TODO have an enum with the layer
            animManager.PlayDeathAnimation(projectileProp);
        }
    }
}

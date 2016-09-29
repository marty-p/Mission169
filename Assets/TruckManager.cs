using UnityEngine;
using System.Collections;
using Slug;

public class TruckManager : MonoBehaviour, IReceiveDamage {

    private FlashUsingMaterial flashRed;
    private Animator animator;
    private TimeUtils timeUtils;
    private bool dead;
    private Vector2 pastPos;
    public GameObject explosions;
    public GameObject debris;
    private Collider2D colliderWhenIntact;
    private EdgeCollider2D colliderWhenDestroyed;
    public EdgeCollider2D colliderRamp;
    private CollectibleDef collectible;
    public SpriteRenderer hideEnemy;
    public EnemySpawner enemySpawner;
    public Berserker[] berserkersOnTheRoof;
    private HealthManager healthManager;
    private Coroutine coroutine;

    void Awake() {
        animator = GetComponent<Animator>();
        timeUtils = GetComponent<TimeUtils>();
        flashRed = GetComponent<FlashUsingMaterial>();
        colliderWhenIntact = GetComponent<Collider2D>();
        colliderWhenDestroyed = GetComponent<EdgeCollider2D>();
        collectible = GetComponentInChildren<CollectibleDef>(true);
        healthManager = GetComponent<HealthManager>();
    }

    public void OnBecameInvisible() {
        gameObject.SetActive(false);
    }

    void Start() {
        pastPos = transform.position;
        coroutine = StartCoroutine("CheckIfMoving");
        healthManager.IgnoreDamages = true;
    }

    public void OnDamageReceived(ProjectileProperties projectileProp, int newHP) {
        if (newHP > 0) {
            flashRed.FlashSlugStyle();
            return;
        } else if (!dead) {
            dead = true;
            flashRed.FlashSlugStyle();
            hideEnemy.enabled = false;
            animator.SetTrigger("destroyed");
            explosions.SetActive(true);
            debris.SetActive(true);
            colliderWhenDestroyed.enabled = true;
            colliderWhenIntact.enabled = false;
            gameObject.tag = "World";
            collectible.gameObject.SetActive(true);
            colliderRamp.enabled = false;
            enemySpawner.gameObject.SetActive(false);
        }
    }

    public void AEShowHideEnemy() {
        for (int i=0; i< berserkersOnTheRoof.Length; i++) {
            berserkersOnTheRoof[i].SetWalkingMode();
        }
        timeUtils.TimeDelay(2, () => {
            if (!dead) {
                hideEnemy.enabled = true;
                enemySpawner.gameObject.SetActive(true);
            }
        });
        StopCoroutine("CheckIfMoving");
    }

    private IEnumerator CheckIfMoving() {
        while (true) {
            if (pastPos.x != transform.position.x) {
                animator.SetBool("driving", true);
            } else {
                animator.SetBool("driving", false);
                healthManager.IgnoreDamages = false;
            }
            pastPos = transform.position;
            yield return new WaitForSeconds(0.2f);
        }
    }

}

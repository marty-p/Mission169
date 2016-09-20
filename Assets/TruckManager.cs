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

    void Awake() {
        animator = GetComponent<Animator>();
        timeUtils = GetComponent<TimeUtils>();
        flashRed = GetComponent<FlashUsingMaterial>();
        colliderWhenIntact = GetComponent<Collider2D>();
        colliderWhenDestroyed = GetComponent<EdgeCollider2D>();
        collectible = GetComponentInChildren<CollectibleDef>(true);
    }

    public void OnBecameInvisible() {
        gameObject.SetActive(false);
    }

    void Start() {
        pastPos = transform.position;
        StartCoroutine("CheckIfMoving");
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
            hideEnemy.enabled = true;
            enemySpawner.gameObject.SetActive(true);
        });
    }

    private IEnumerator CheckIfMoving() {
        while (true) {
            if (pastPos.x != transform.position.x) {
                animator.SetBool("driving", true);
            } else {
                animator.SetBool("driving", false);
            }
            pastPos = transform.position;

            yield return new WaitForSeconds(0.1f);
        }
    }

}

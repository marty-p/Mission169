using UnityEngine;
using System.Collections;
using Slug;
namespace Mission169
{
    public class TruckManager : MonoBehaviour, IReceiveDamage
    {

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
        public Transform[] berserkerOnTheRoofPos;
        private Berserker[] berserkersOnTheRoof;
        private HealthManager healthManager;
        private Coroutine coroutine;
        private ObjectPoolScript berserkerPool;

        void Awake()
        {
            animator = GetComponent<Animator>();
            timeUtils = GetComponent<TimeUtils>();
            flashRed = GetComponent<FlashUsingMaterial>();
            colliderWhenIntact = GetComponent<Collider2D>();
            colliderWhenDestroyed = GetComponent<EdgeCollider2D>();
            collectible = GetComponentInChildren<CollectibleDef>(true);
            healthManager = GetComponent<HealthManager>();
            berserkerPool = GetComponentInChildren<ObjectPoolScript>();
            berserkersOnTheRoof = new Berserker[3];
        }

        void Start()
        {
            for (int i = 0; i < berserkersOnTheRoof.Length; i++)
            {
                GameObject berserkRoot = berserkerPool.GetPooledObject();
                berserkRoot.transform.position = berserkerOnTheRoofPos[i].position;
                berserkersOnTheRoof[i] = berserkRoot.GetComponentInChildren<Berserker>();
                berserkersOnTheRoof[i].SetWalkingMode(false);
            }
            colliderRamp.enabled = false;
        }

        void OnEnable()
        {
            pastPos = transform.position;
            coroutine = StartCoroutine("CheckIfMoving");
            healthManager.IgnoreDamages = true;
        }

        public void OnDamageReceived(ProjectileProperties projectileProp, int newHP)
        {
            if (newHP > 0)
            {
                flashRed.FlashSlugStyle();
                return;
            }
            else if (!dead)
            {
                dead = true;
                flashRed.FlashSlugStyle();
                hideEnemy.enabled = false;
                animator.SetTrigger("destroyed");
                explosions.SetActive(true);
                debris.SetActive(true);
                colliderWhenDestroyed.enabled = true;
                gameObject.tag = "World";
                collectible.gameObject.SetActive(true);
                colliderRamp.enabled = false;
                enemySpawner.gameObject.SetActive(false);
            }
        }

        public void AEShowHideEnemy()
        {
            for (int i = 0; i < berserkersOnTheRoof.Length; i++)
            {
                berserkersOnTheRoof[i].SetWalkingMode(true);
            }
            timeUtils.TimeDelay(2, () =>
            {
                if (!dead)
                {
                    colliderWhenIntact.enabled = false;
                    hideEnemy.enabled = true;
                    colliderRamp.enabled = true;
                    print("ONE SPAWN!");
                    enemySpawner.StartPeriodicSpawning();
                }
            });
            StopCoroutine("CheckIfMoving");
        }

        private IEnumerator CheckIfMoving()
        {
            while (true)
            {
                if (pastPos.x != transform.position.x)
                {
                    animator.SetBool("driving", true);
                }
                else
                {
                    animator.SetBool("driving", false);
                    healthManager.IgnoreDamages = false;
                }
                pastPos = transform.position;
                yield return new WaitForSeconds(0.2f);
            }
        }
    }
}

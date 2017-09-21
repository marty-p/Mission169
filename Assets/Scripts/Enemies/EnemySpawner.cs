using System.Collections;
using UnityEngine;
using SlugLib;

namespace Mission169
{
    public enum SpawnerType
    {
        CameraBased,
        CameraBlocker,
        Manual,
    } 

    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] SpawnerType spawnerType = SpawnerType.CameraBased;

        public ObjectPoolScript enemyPool;
        public GameObject singleEnemyPrefab;
        public Transform posToGoAtSpawn;
        public float goToSpeedFactor = 1;

        [Header("Manual parameters")]
        [SerializeField] bool spawnAgainAtDeath;
        [Tooltip("When 0 only one enemy is spawned")]
        [SerializeField] float spawningInterval;

        private HealthManager enemyHealthManager;
        private GameObject enemy;
        private Coroutine spawnCoroutine;

        #region Unity callbacks

        void Start()
        {
            if (singleEnemyPrefab && enemy == null)
            {
                InstantiateSingleEnemyPrefab();
            }

            // when debuggin and stating with the camera not from the beginning you don't want all the spawners to spawn their mobs
            if (spawnerType == SpawnerType.CameraBased && IsCameraEdgePastSpawner())
            {
                enabled = false;
            }
        }

        private void Update()
        {
            if (spawnerType != SpawnerType.CameraBased)
            {
                enabled = false;
                return;
            }

            if (IsCameraEdgePastSpawner())
            {
                Spawn();
                enabled = false;
            }
        }

        #endregion

        #region Public methods

        public void Spawn()
        {
            if (singleEnemyPrefab != null)
            {
                if (enemy == null)
                {
                    InstantiateSingleEnemyPrefab();
                }

                enemy.SetActive(true);
                enemy.transform.position = transform.position;
            }
            else if (enemyPool != null)
            {
                enemy = enemyPool.GetPooledObject();
                enemy.transform.position = transform.position;

                if (spawnerType == SpawnerType.Manual && spawnAgainAtDeath)
                {
                    spawnCoroutine = StartCoroutine(RespawnUponDeathCoroutine());
                }
            }

            enemyHealthManager = enemy.GetComponentInChildren<HealthManager>();

            if (spawnerType == SpawnerType.CameraBlocker)
            {
                CameraUtils.EnableFollow(false);
                StartCoroutine(AllowCameraFollowCoroutine());
            }

            if (posToGoAtSpawn != null)
            {
                StartCoroutine(GoToCoroutine(posToGoAtSpawn.position.x));
            }
        }

        public void StartPeriodicSpawning()
        {
            spawnCoroutine = StartCoroutine(SpawnEveryXsecondsCoroutine(spawningInterval));
        }

        public void StopSpawning()
        {
            if (spawnCoroutine != null)
            {
                StopCoroutine(spawnCoroutine);
            }
        }

        public bool IsEnemyAlive()
        {
            return enemyHealthManager.currentHP > 0;
        }

        public GameObject GetEnemy()
        {
            return enemy;
        }

        #endregion Private Methods

        private bool IsCameraEdgePastSpawner()
        {
            return CameraUtils.GetRightEdgeWorldPosition() + 0.2f >= transform.position.x;
        }

        private IEnumerator GoToCoroutine(float posX)
        {
            while (!Mathf.Approximately(enemy.transform.position.x, posX))
            {
                float newPosX = Mathf.MoveTowards(enemy.transform.position.x, posX, 0.2f * Time.deltaTime * goToSpeedFactor);
                enemy.transform.position = new Vector2(newPosX, enemy.transform.position.y);
                yield return new WaitForEndOfFrame();
            }
        }

        private IEnumerator SpawnEveryXsecondsCoroutine(float interval)
        {
            while (true)
            {
                yield return new WaitForSeconds(interval);
                Spawn();
            }
        }

        private IEnumerator RespawnUponDeathCoroutine()
        {
            while (IsEnemyAlive())
            {
                yield return new WaitForSeconds(1f);
            }

            Spawn();
        }

        private IEnumerator AllowCameraFollowCoroutine()
        {
            while (IsEnemyAlive())
            {
                yield return new WaitForSeconds(1f);
            }

            CameraUtils.EnableFollow(true);
        }

        private void InstantiateSingleEnemyPrefab()
        {
            enemy = Instantiate(singleEnemyPrefab);
            enemy.transform.parent = transform;
            enemy.SetActive(false);
        }
    }
}

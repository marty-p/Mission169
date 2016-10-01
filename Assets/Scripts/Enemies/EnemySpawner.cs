using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    public ObjectPoolScript enemyPool;
    public GameObject singleEnemyPrefab;
    private GameObject enemy;
    private new BoxCollider2D collider;
    public Transform posToGoAtSpawn;
    [Tooltip("When 0 only one enemy is spawned")]
    public float spawningInterval;

    public bool EnemyAlive() {
        return enemy.active;
    }
    public GameObject GetEnemy() {
        return enemy;
    }

    void Awake () {
        collider = GetComponent<BoxCollider2D>();
        if (singleEnemyPrefab) {
            enemy = Instantiate(singleEnemyPrefab);
            enemy.SetActive(false);
        }
	}

    void OnEnable() {
        InitEnemy();
        if (spawningInterval != 0) {
            StartCoroutine("SpawnEveryXsecondsCoroutine", spawningInterval);
        }
    }

    void InitEnemy() {
        if (singleEnemyPrefab != null) {
            enemy.SetActive(true);
            enemy.transform.position = transform.position;
            if (spawningInterval != 0) {
                print("Can't use spawning interval and single enemy spawn. Use pool spawning");
            }
        } else if (enemyPool != null) {
            enemy = enemyPool.GetPooledObject();
            enemy.transform.position = transform.position;
        } else {
        }

        if (posToGoAtSpawn != null) {
            StartCoroutine("GoToCoroutine", posToGoAtSpawn.position.x);
        }

    }

    void OnTriggerEnter2D(Collider2D col) {
        if (col.tag == "Player") {
            InitEnemy();
            collider.enabled = false;
        }
    }


    private IEnumerator GoToCoroutine(float posX) {
        while (!Mathf.Approximately(enemy.transform.position.x, posX)) {
            float newPosX = Mathf.MoveTowards(enemy.transform.position.x, posX, 0.4f*Time.deltaTime);
            enemy.transform.position = new Vector2(newPosX, enemy.transform.position.y);
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator SpawnEveryXsecondsCoroutine(float interval) {
        while (this.enabled) {
            yield return new WaitForSeconds(interval);
            InitEnemy();
        }
    }
}



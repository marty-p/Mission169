using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    public ObjectPoolScript enemyPool;
    public GameObject singleEnemyPrefab;
    private GameObject enemy;
    private new BoxCollider2D collider;

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

    void InitEnemy() {
        if (singleEnemyPrefab != null) {
            enemy.SetActive(true);
        } else if (enemyPool != null) {
            enemy = enemyPool.GetPooledObject();
            enemy.transform.position = transform.position;
        } else {
            print("Enemy Spawner, no single enemy or pool passed");
        }
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (col.tag == "Player") {
            InitEnemy();
            collider.enabled = false;
        }
    }

    void OnEnable() {
        InitEnemy();
    }

}



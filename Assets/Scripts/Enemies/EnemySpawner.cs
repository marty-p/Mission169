using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    public ObjectPoolScript enemyPool;
    private new BoxCollider2D collider;
    private GameObject enemy;

    public bool EnemyAlive() {
        return enemy.active;
    }

    void Awake () {
        collider = GetComponent<BoxCollider2D>();
	}

    void InitEnemy() {
        enemy = enemyPool.GetPooledObject();
        enemy.transform.position = transform.position;
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



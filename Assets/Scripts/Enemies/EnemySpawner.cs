using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    public ObjectPoolScript enemyPool;
    private new BoxCollider2D collider;
    private GameObject enemy;

    public bool EnemyAlive() {
        return enemy.active;
    }

    void Start () {
        collider = GetComponent<BoxCollider2D>();
	}
	
    void OnTriggerEnter2D(Collider2D col) {
        if (col.tag == "Player") {
            InitSoldier();
            collider.enabled = false;
        }
    }

    void OnEnable() {
        InitSoldier();
    }

    void InitSoldier() {
        enemy = enemyPool.GetPooledObject();
        enemy.transform.position = transform.position;
    }
}



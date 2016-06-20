using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    public ObjectPoolScript enemyPool;
    private new BoxCollider2D collider;


    void Start () {
        collider = GetComponent<BoxCollider2D>();
	}
	
    void OnTriggerEnter2D(Collider2D col) {
        if (col.tag == "Player") {
            InitSoldier();
          //  collider.enabled = false;
        }
    }

    void InitSoldier() {
        GameObject soldier = enemyPool.GetPooledObject();
        soldier.transform.position = transform.position;
    }

}

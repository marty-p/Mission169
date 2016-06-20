using UnityEngine;

public class EnemyInit : MonoBehaviour {

    void OnEnable() {
        gameObject.layer = 10; //TODO
        transform.localPosition = Vector3.zero;
    }

}

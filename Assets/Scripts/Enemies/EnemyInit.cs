using UnityEngine;
using Slug;

public class EnemyInit : MonoBehaviour {

    public SlugLayers initLayer;
    private HealthManager healthManager;

    void Awake() {
        healthManager = GetComponent<HealthManager>();
    }

    void OnEnable() {
        gameObject.layer = (int) initLayer;
        transform.localPosition = Vector3.zero;
        healthManager.currentHP = healthManager.maxHP;
    }

}

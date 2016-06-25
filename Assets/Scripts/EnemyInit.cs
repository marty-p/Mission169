using UnityEngine;
using Slug;

public class EnemyInit : MonoBehaviour {

    public SlugLayers initLayer;

    void OnEnable() {
        gameObject.layer = (int) initLayer;
        transform.localPosition = Vector3.zero;
    }

}

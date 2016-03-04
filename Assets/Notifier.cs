using UnityEngine;

public class Notifier : MonoBehaviour {

    private ISight[] sightComponents;

    void Start() {
        sightComponents = GetComponentsInParent<ISight>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            foreach(ISight c in sightComponents) {
                c.OnPlayerSpotted(col.transform);
            }
        }
    }

}

using SlugLib;
using UnityEngine;

namespace Mission169
{
    public class CampFire : MonoBehaviour
    {
        public GameObject soldier1;
        public GameObject soldier2;
        public EnemySpawner spawn1;
        public EnemySpawner spawn2;
        private BoxCollider2D boxCol;

        void Awake()
        {
            boxCol = GetComponent<BoxCollider2D>();
        }

        void OnTriggerEnter2D(Collider2D col)
        {
            if (col.tag == "Player")
            {
                PlayerCrashesTheParty();
                boxCol.enabled = false;
            }
        }

        private void PlayerCrashesTheParty()
        {
            soldier1.SetActive(false);
            soldier2.SetActive(false);
            spawn1.enabled = true;
            spawn2.enabled = true;
            EventManager.TriggerEvent(GlobalEvents.PlayerSpawned);
        }

    }
}
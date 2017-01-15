using UnityEngine;
using SlugLib;

namespace Mission169 {
    public abstract class EnemyEvents : MonoBehaviour {

       void Start() {
            EventManager.Instance.StartListening(GlobalEvents.PlayerDead, OnPlayerDies);
            EventManager.Instance.StartListening(GlobalEvents.PlayerSpawned, OnPlayerSpawned);
            EventManager.Instance.StartListening(GlobalEvents.MissionSuccess, OnMissionSuccess);
        }

        public abstract void OnInit();
        public abstract void OnPlayerDies();
        public abstract void OnPlayerSpawned();
        public abstract void OnMissionSuccess();
        public abstract void OnHit(ProjectileProperties proj);
        public abstract void OnDead(ProjectileProperties proj);
    }
}


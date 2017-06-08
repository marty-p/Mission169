using UnityEngine;
using Slug;
using SlugLib;

namespace Mission169 {
    public class EnemyBuilder : MonoBehaviour, IReceiveDamage {
        public int maxHP = 1;
        public SlugLayers initLayer;
        public int pointsValue = 100;
        private EnemyEvents enemyEvents;
        private HealthManager healthManager;
        public AudioClip[] audioClips;

        void Awake() {
            healthManager = gameObject.AddComponent<HealthManager>();
            healthManager.maxHP = maxHP;
            SlugAudioManager audioManager = gameObject.AddComponent<SlugAudioManager>();
            audioManager.InitAudioClips(audioClips);
            gameObject.AddComponent<TimeUtils>();
            gameObject.AddComponent<Blink>();
            enemyEvents = GetComponent<EnemyEvents>();
            if (enemyEvents == null) {
                Debug.LogError("missing EnemyGetsDamage");
            }
        }

        void OnEnable() {
            gameObject.layer = (int)initLayer;
            transform.localPosition = Vector3.zero;
            enemyEvents.OnInit();
        }

        public void OnDamageReceived(ProjectileProperties projectileProp, int newHP) {
            if (newHP > 0) {
                enemyEvents.OnHit(projectileProp);
                return;
            } else {
                enemyEvents.OnDead(projectileProp);
                //To ignore collision with projectiles during death anim but still be 'physic'
                gameObject.layer = 2;
                EventManager.TriggerEvent(GlobalEvents.PointsEarned, pointsValue);
                EventManager.TriggerEvent(GlobalEvents.SoldierDead);
            }
        }
    }
}

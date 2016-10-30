using UnityEngine;
using Slug;
using SlugLib;

public class EnemyBuilder : MonoBehaviour, IReceiveDamage {

    public int maxHP = 1;
    public SlugLayers initLayer;
    public int pointsValue = 100;
    private HealthManager healthManager;
    public AudioClip[] audioClips;


    void Awake() {
        healthManager = gameObject.AddComponent<HealthManager>();
        SlugAudioManager audioManager = gameObject.AddComponent<SlugAudioManager>();
        audioManager.InitAudioClips(audioClips);
        gameObject.AddComponent<TimeUtils>();
        gameObject.AddComponent<Blink>();
    }


    void OnEnable() {
        gameObject.layer = (int)initLayer;
        transform.localPosition = Vector3.zero;
        healthManager.currentHP = maxHP;
    } 

    public void OnDamageReceived(ProjectileProperties projectileProp, int newHP) {
        if (newHP > 0) {
            return;
        } else {
            //To ignore collision with projectiles during death anim but still be 'physic'
            gameObject.layer = 2;
            EventManager.Instance.TriggerEvent(GlobalEvents.PointsEarned, pointsValue);
            EventManager.Instance.TriggerEvent(GlobalEvents.SoldierDead);
        }
    }


}

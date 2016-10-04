using UnityEngine;
using Utils;

public class Tower : MonoBehaviour, IReceiveDamage {

    public GameObject doors;
    public MissileLauncher missileLauncher;
    public GameObject explosion;
    public GameObject debris;
    private Animator anim;
    private TimeUtils timeUtils;
    private RetVoidTakeVoid OpenAnimDoneCB;
    private FlashUsingMaterial flashRed;
    private SlugAudioManager audioManager;
    public TowerAgressiveMode agressiveMode;
    private bool dead;
    public bool Dead { get { return dead; } }

    void Awake() {
        anim = GetComponent<Animator>();
        timeUtils = GetComponent<TimeUtils>();
        flashRed = GetComponent<FlashUsingMaterial>();
        audioManager = GetComponentInChildren<SlugAudioManager>();
    }

    public void Fire(float speedFactor = 1) {
        missileLauncher.Shoot(speedFactor);
    }

    public void OpenTower(RetVoidTakeVoid OpenCB) {
        OpenAnimDoneCB = OpenCB;
        doors.SetActive(true);
        anim.enabled = true;
        timeUtils.TimeDelay(3, () => {
            doors.SetActive(false);
            anim.SetTrigger("open_curtain"); }
        );
    }

    public void AEActivateMissileLauncher() {
        missileLauncher.gameObject.SetActive(true);
    }

    public void AEMissileLauncherReady() {
        if (OpenAnimDoneCB != null) {
            OpenAnimDoneCB();
        }
    }

    public void EnableAgressiveMode() {
        agressiveMode.EnableAgressiveMode(true);
    }

    public void OnDamageReceived(ProjectileProperties projectileProp, int newHP) {
        if (newHP > 1) {
            flashRed.FlashSlugStyle();
            EventManager.TriggerEvent("add_points", 100);
            audioManager.PlaySound(0);
        } else {
            Die();
        }
    }

    private void Die() {
        if (!dead){
            audioManager.PlaySound(1);
            anim.SetTrigger("destroyed");
            explosion.SetActive(true);
            missileLauncher.gameObject.SetActive(false);
            debris.SetActive(true);
            dead = true;
        }
    }
}

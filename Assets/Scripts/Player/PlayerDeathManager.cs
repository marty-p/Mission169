using UnityEngine;

public class PlayerDeathManager : MonoBehaviour, IReceiveDamage {

    private InputManager inputManager;
    private AnimationManager animManager;
    private SlugPhysics physic;
    private MovementManager movementManager;
    private Blink blink;
    private HealthManager health;
    private TimeUtils timeUtils;
    private SpriteRenderer[] spriteRenderers;
    private FlashUsingMaterial flashBright;
    private FlipPlayerIndicator playerIndicator;

    public SlugAudioManager audioManager;
    public int ignoreDamagesDuration = 3;

    void Awake() {
        inputManager = GetComponent<InputManager>();
        animManager = GetComponent<AnimationManager>();
        blink = GetComponent<Blink>();
        physic = GetComponent<SlugPhysics>();
        movementManager = GetComponent<MovementManager>();
        health = GetComponent<HealthManager>();
        timeUtils = GetComponent<TimeUtils>();
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        flashBright = GetComponent<FlashUsingMaterial>();
        playerIndicator = GetComponentInChildren<FlipPlayerIndicator>();
        playerIndicator.gameObject.SetActive(false);
    }

    public void OnDamageReceived(ProjectileProperties projectileProp, int newHP) {
        if (newHP > 0) {
            return;
        } else {
            animManager.PlayDeathAnimation(projectileProp, DeathAnimCB);
            movementManager.StopMoving();
            inputManager.enabled = false;
            gameObject.layer = 2;
            if (projectileProp.type == ProjectileType.Grenade) {
                physic.SetVelocityY(3);
                physic.SetVelocityX(-transform.right.x/3);
            }
            audioManager.PlaySound(2);
        }
    }

    public void DeathAnimCB() {
        blink.BlinkPlease(NotifyDeath);
    }

    private void NotifyDeath() {
        setPlayerVisible(false);
        EventManager.TriggerEvent("player_death");
    }

    public void SpawnPlayer() {
        setPlayerVisible(true);
        inputManager.enabled = true;
        gameObject.layer = 8;
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        animManager.ResetTopAnimator();
        animManager.ResetBottomAnimator();
        animManager.PlaySpawnAnim();
        health.IgnoreDamages = true;
        playerIndicator.SetVisible(true);
        flashBright.FlashForXSecs(ignoreDamagesDuration);
        timeUtils.TimeDelay(ignoreDamagesDuration,() => 
        {       health.IgnoreDamages = false;
                playerIndicator.SetVisible(false);
        });
    }

    private void setPlayerVisible(bool visible) {
        for (int i =0; i< spriteRenderers.Length; i++) {
            spriteRenderers[i].enabled = visible;
        }
    }

}

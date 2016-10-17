using UnityEngine;
using Slug;

public class MarcoAttackManager : MonoBehaviour {

    private IAttack[] FireArmAttacks;
    private IAttack currentFireArmAttack;
    private AttackKnife MeleeAttack;
    private IAttack grenadeAttack;
    // TODO currentAttackID ...
    private int currentAttackID = 1;
    public int bulletCount;
    public int grenadeCount = 10;
    private RuntimeAnimatorController gunAnimController;
    private FlashUsingMaterial flashBlue;

    public string victimsTag = "enemy";
    public LayerMask enemyLayer;
    public Animator topBodyAnimator;

    public Transform grenadeInitialPositionStand;
    public Transform grenadeInitialPositionCrouch;
    public SlugAudioManager audioManager;

    private Vector3 projInitialPosition;
    private Vector3 dir;
    private MovementManager movementManager;

    void Start() {
        gunAnimController = topBodyAnimator.runtimeAnimatorController;
        MeleeAttack = GetComponentInChildren<AttackKnife>();
        FireArmAttacks = GetComponentsInChildren<IAttack>(true);
        grenadeAttack = GetComponent<MarcoAttackGrenade>();
        movementManager = GetComponentInParent<MovementManager>();
        flashBlue = GetComponent<FlashUsingMaterial>();

        currentFireArmAttack = FireArmAttacks[1];
    }

    public void PrimaryAttack() {
        Vector3 unUsed = Vector3.zero;
        if (!MeleeAttack.InProgress()) {
            if (InRangeForKnife()) {
                MeleeAttack.Execute(victimsTag, Vector3.zero, Vector3.zero);
            } else {
                //TODO remove this projectileInitialPos parameter from Execute
                currentFireArmAttack.Execute(victimsTag, unUsed, unUsed);
            }
        }
    }

    public void SecondaryAttack() {
        if (grenadeCount > 0) {
            grenadeCount--;
            EventManager.Instance.TriggerEvent("grenade_thrown", grenadeCount);

            Vector3 grenadeInitialPos;
            if (movementManager.body == BodyPosture.Crouch) {
                grenadeInitialPos = grenadeInitialPositionCrouch.position;
            } else {
                grenadeInitialPos = grenadeInitialPositionStand.position;
            }
            grenadeAttack.Execute(victimsTag, Vector3.zero, grenadeInitialPos);
        }
    }

    public void RestoreGrenade() {
        grenadeCount = 10;
        EventManager.Instance.TriggerEvent("grenade_thrown", grenadeCount);
    }

    public void UpdateBulletCount(int newBulletCount = 0) {
        if (newBulletCount == 0) {
            bulletCount--;
        } else {
            bulletCount = bulletCount + newBulletCount;
        }
        if (bulletCount < 1) {
            SetDefaultAttack();
        }
        // way to update the UI
        EventManager.Instance.TriggerEvent("bullet_shot", bulletCount);
    }

    public void SetAttack(int attackID, RuntimeAnimatorController attackAnimController) {
        currentFireArmAttack = FireArmAttacks[attackID];
        topBodyAnimator.runtimeAnimatorController = attackAnimController;
    }

    public void SetDefaultAttack() {
        currentFireArmAttack = FireArmAttacks[1];
        topBodyAnimator.runtimeAnimatorController = gunAnimController;
    }

    private bool InRangeForKnife() {
        RaycastHit2D[] hits = Physics2D.LinecastAll(transform.position,
                new Vector2(transform.position.x + transform.right.x * 0.35f, transform.position.y), enemyLayer);

        Debug.DrawLine(transform.position,
                        new Vector2(transform.position.x + transform.right.x * 0.35f, transform.position.y),
                        Color.cyan);

        for (int i = 0; i < hits.Length; i++) {
            if (hits[i].collider.tag == victimsTag) {
                return true;
            }
        }
        return false;
    }

}

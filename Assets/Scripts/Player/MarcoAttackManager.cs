using UnityEngine;
using Slug;

public class MarcoAttackManager : MonoBehaviour {

    private IAttack[] FireArmAttacks;
    private IAttack MeleeAttack;
    private IAttack grenadeAttack;
    // TODO currentAttackID ...
    private int currentAttackID = 2;
    public int bulletCount;
    private RuntimeAnimatorController gunAnimController;
    private FlashUsingMaterial flashBlue;

    public string victimsTag = "enemy";
    public Animator topBodyAnimator;

    public Transform grenadeInitialPositionStand;
    public Transform grenadeInitialPositionCrouch;

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
    }

    public void PrimaryAttack() {
        Vector3 unUsed = Vector3.zero;
        if (InRangeForKnife()) {
            MeleeAttack.Execute(victimsTag);
        } else if (bulletCount > 0) {
            //TODO remove this projectileInitialPos parameter from Execute
            FireArmAttacks[currentAttackID].Execute(victimsTag, unUsed, unUsed);
        } else {
            FireArmAttacks[1].Execute(victimsTag, unUsed, unUsed);
        }
    }

    public void SecondaryAttack() {
        Vector3 grenadeInitialPos;
        if (movementManager.body == BodyPosture.Crouch) {
            grenadeInitialPos = grenadeInitialPositionCrouch.position;
        } else {
            grenadeInitialPos = grenadeInitialPositionStand.position;
        }
        grenadeAttack.Execute(victimsTag, Vector3.zero, grenadeInitialPos);
    }

    public void UpdateBulletCount() {
        bulletCount--;
        if (bulletCount < 1) {
            topBodyAnimator.runtimeAnimatorController = gunAnimController;
        }
    }

    public void OnTriggerEnter2D(Collider2D collider) {
        if (collider.tag == "Collectible") {
            CollectibleDef def = collider.gameObject.GetComponent<CollectibleDef>();
            topBodyAnimator.runtimeAnimatorController = def.animController;
            currentAttackID = def.attackID;
            bulletCount = def.bulletCount;
            flashBlue.FlashForXSecs(0.16f);
        }
    }

    private bool InRangeForKnife() {
        RaycastHit2D[] hits = Physics2D.LinecastAll(transform.position,
                new Vector2(transform.position.x + transform.right.x * 0.35f, transform.position.y));

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

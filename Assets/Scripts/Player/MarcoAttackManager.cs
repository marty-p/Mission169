using UnityEngine;

public class MarcoAttackManager : MonoBehaviour {
    private IAttack MeleeAttack;
    private IAttack FireArmAttack;
    private IAttack grenadeAttack;
    private PhysicsSlugEngine physics;

    public string victimsTag = "enemy";
    public Animator anim;
    private bool inRangeForKnife;
    private bool lookingUp;
    private bool sitting;
    private bool walking;
    public Transform bulletInitialPosition;
    public Transform bulletInitialPositionSitting;
    public Transform bulletInitialPositionWalking;
    public Transform grenadeInitialPositionStand;
    public Transform grenadeInitialPositionCrouch;
    public Transform bulletInitialPositionLookingUp;
    public Transform bulletInitialPositionLookingDown;
    private Vector3 projInitialPosition;
    private Vector3 dir;
    private MovementManager movementManager;

    void Start() {
        MeleeAttack = (IAttack)GetComponent("AttackKnife");
        FireArmAttack = (IAttack)GetComponent("BasicGunAttack");
        grenadeAttack = GetComponent<MarcoAttackGrenade>();
        physics = GetComponentInParent<PhysicsSlugEngine>();
        movementManager = GetComponentInParent<MovementManager>();
    }

    public void PrimaryAttack() {
        if (inRangeForKnife) {
            MeleeAttack.Execute(victimsTag);
            inRangeForKnife = false;
        } else {
            Vector3 projInitPos = GetProjPosInit();
            FireArmAttack.Execute(victimsTag, movementManager.lookingDirection, projInitPos);
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

    private Vector3 GetProjPosInit() {
        if (movementManager.body == BodyPosture.Crouch) {
            return bulletInitialPositionSitting.position;
        } else if (movementManager.lookingDirection == Vector2.down) {
            return bulletInitialPositionLookingDown.position;
        } else if (movementManager.IsInMotion() && movementManager.lookingDirection != Vector2.up) {
            return bulletInitialPositionWalking.position;
        } else if (movementManager.lookingDirection == Vector2.up) {
            return bulletInitialPositionLookingUp.position;
        } else {
            return bulletInitialPosition.position;
        }
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (col.tag == "enemy") {
            inRangeForKnife = true;
        }
    }

    void OnTriggerExit2D(Collider2D col) {
        if (col.tag == "enemy") {
            inRangeForKnife = false;
        }
    }

}

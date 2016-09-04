using UnityEngine;

public class BasicGunAttack : MonoBehaviour, IAttack {

    public Animator anim;
    public RuntimeAnimatorController attackAnimatorController;
    public ObjectPoolScript bullettPool;
    public MovementManager movementManager;
    public SlugAudioManager audioManager;

    public Transform bulletInitialPosition;
    public Transform bulletInitialPositionSitting;
    public Transform bulletInitialPositionWalking;
    public Transform bulletInitialPositionLookingUp;
    public Transform bulletInitialPositionLookingDown;

    public void Execute(string victimTag, Vector3 unused, Vector3 unused2) {
        anim.runtimeAnimatorController = attackAnimatorController;
        anim.SetTrigger("fire");
        GameObject bulletGameObject = bullettPool.GetPooledObject();
        bulletGameObject.transform.position = GetProjPosInit();
        bulletGameObject.transform.right = movementManager.lookingDirection;
        IProjectile bullet = bulletGameObject.GetComponent<IProjectile>();
        bullet.Launch(victimTag);
        audioManager.PlaySound(0);
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


}

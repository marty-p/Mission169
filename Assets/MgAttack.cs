using UnityEngine;
using System.Collections;

public class MgAttack : MonoBehaviour, IAttack {

    public Animator anim;
    public RuntimeAnimatorController mgAnimatorController;
    public ObjectPoolScript bullettPool;
    public MarcoAttackManager attackManager;
    private TimeUtils timeUtils;

    public MachineGunAnimationEvents mgAE;
    private bool withYoffset;
    private string vicTag;
    public Transform[] bulletDiagUpPos;
    public Transform[] bulletDiagDownPos;
    public Transform bulletHoriz;
    public Transform bulletUp;
    public Transform bulletSitting;
    public Transform bulletDown;

    public void Start() {
        timeUtils = GetComponent<TimeUtils>();
        mgAE.SetMgHorizontalCB(() => Shoot(bulletHoriz));
        mgAE.SetMgDiagUp((int animIndex) => Shoot(bulletDiagUpPos[animIndex]));
        mgAE.SetMgDiagDown((int animIndex) => Shoot(bulletDiagDownPos[animIndex]));
        mgAE.SetMgUpCB(() => Shoot(bulletUp));
        mgAE.SetMgSittingCB(() => Shoot(bulletSitting));
        mgAE.SetMgDownCB(() => Shoot(bulletDown));
    }

    private void Shoot(Transform initTransform) {
        GameObject bulletGameObject = bullettPool.GetPooledObject();

        bulletGameObject.transform.position = initTransform.position;
        bulletGameObject.transform.rotation = initTransform.rotation;

        if (withYoffset) {
            bulletGameObject.transform.Translate(0, 0.055f, 0, Space.Self);
            withYoffset = false;
        } else {
            withYoffset = true;
        }

        IProjectile bullet = bulletGameObject.GetComponent<IProjectile>();
        bullet.Launch(vicTag);
        attackManager.UpdateBulletCount();
    }

    public void Execute(string victimTag, Vector3 unused , Vector3 unused2) {
        vicTag = victimTag;

        if (timeUtils != null) {
            timeUtils.TimeDelay(0.3f, ()=>anim.SetBool("machineGunning", false));
        }

        anim.runtimeAnimatorController = mgAnimatorController;
        anim.SetBool("machineGunning", true);
    }


}

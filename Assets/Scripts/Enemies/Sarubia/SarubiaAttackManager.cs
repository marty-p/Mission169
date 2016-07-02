using UnityEngine;
using Utils;

public class SarubiaAttackManager : MonoBehaviour {

    private SarubiaAnimationManager animManager;
    private ObjectPoolScript projectilePool;
    public Transform projectileInitialPos;

	void Awake () {
        animManager = GetComponent<SarubiaAnimationManager>();
        projectilePool = GetComponent<ObjectPoolScript>();
	}

    public void PrimaryAttack () {
        animManager.StartShootAnim(BadaBoom);
    }
	
    private void  BadaBoom() {
        GameObject projectileGameObject = projectilePool.GetPooledObject();
        IProjectile projectile = projectileGameObject.GetComponent<IProjectile>();
        SlugPhysics projPhysic = projectileGameObject.GetComponent<SlugPhysics>();
        projectileGameObject.transform.position = projectileInitialPos.position;
        projectile.Launch("Player");
        projPhysic.SetVelocityX(1);
    }

}

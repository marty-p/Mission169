using System.Collections;
using UnityEngine;

public class SarubiaAttackManager : MonoBehaviour {

    private SarubiaAnimationManager animManager;
    private ObjectPoolScript projectilePool;
    public Transform projectileInitialPos;

    void Awake() {
        animManager = GetComponent<SarubiaAnimationManager>();
        projectilePool = GetComponent<ObjectPoolScript>();
    }

    void OnBecameVisible() {
        StartCoroutine(WaveUpdate());
    }

    private IEnumerator WaveUpdate() {
        while (true) {
            PrimaryAttack();
            yield return new WaitForSeconds(3);
        }
    }


    public void PrimaryAttack() {
        animManager.StartShootAnim(BadaBoom);
    }

    private void BadaBoom() {
        GameObject projectileGameObject = projectilePool.GetPooledObject();
        IProjectile projectile = projectileGameObject.GetComponent<IProjectile>();
        SlugPhysics projPhysic = projectileGameObject.GetComponent<SlugPhysics>();
        projectileGameObject.transform.position = projectileInitialPos.position;
        projectile.Launch("Player");
        projPhysic.SetVelocityX(transform.right.x * 0.4f);
    }

}

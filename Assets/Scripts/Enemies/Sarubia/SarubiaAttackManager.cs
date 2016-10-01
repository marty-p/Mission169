using System.Collections;
using UnityEngine;

public class SarubiaAttackManager : MonoBehaviour {

    private SarubiaAnimationManager animManager;
    private ObjectPoolScript projectilePool;
    private SlugAudioManager audioManager;
    public Transform projectileInitialPos;
    public Animator explosionAnim;

    void Awake() {
        animManager = GetComponent<SarubiaAnimationManager>();
        projectilePool = GetComponent<ObjectPoolScript>();
        audioManager = GetComponentInChildren<SlugAudioManager>();
    }

    void OnBecameVisible() {
        StartCoroutine(WaveUpdate());
    }

    private IEnumerator WaveUpdate() {
        while (true) {
            PrimaryAttack();
            yield return new WaitForSeconds(4);
        }
    }

    public void PrimaryAttack() {
        if (!animManager.IsMoving()) {
            animManager.StartShootAnim(BadaBoom);
        }
    }

    public void AEexplosion() {
        explosionAnim.Play("explosion");
    }

    private void BadaBoom() {
        GameObject projectileGameObject = projectilePool.GetPooledObject();
        IProjectile projectile = projectileGameObject.GetComponent<IProjectile>();
        SlugPhysics projPhysic = projectileGameObject.GetComponent<SlugPhysics>();
        projectileGameObject.transform.position = projectileInitialPos.position;
        projectile.Launch("Player");
        projPhysic.SetVelocityX(transform.right.x * 0.4f);
        audioManager.PlaySound(2);
    }

}

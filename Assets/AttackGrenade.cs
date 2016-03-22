using UnityEngine;

public class AttackGrenade : MonoBehaviour, IObserver {

    public ObjectPoolScript grenadePool;
    public Animator animator;
    public Transform initialPosition;

    public void Observe(SlugEvents ev) {
        if (ev == SlugEvents.Grenade) {
            GrenadeAttack();
        }
    }

    private void GrenadeAttack() {
        animator.SetTrigger("grenade");
        GameObject grenadeGameObject = grenadePool.GetPooledObject();
        GrenadeController grenadeSpecific = grenadeGameObject.GetComponent<GrenadeController>();
        grenadeSpecific.transform.position = initialPosition.position;
        grenadeSpecific.Init();
        grenadeSpecific.Throw(transform.right);
    }	

}

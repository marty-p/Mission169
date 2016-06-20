using UnityEngine;
using Utils;

public class SarubiaAnimationManager : MonoBehaviour {

    private Animator anim;
    private RetVoidTakeVoid ShootCB;


    void Awake () {
        anim = GetComponent<Animator>();
	}

    public void StartShootAnim (RetVoidTakeVoid cb) {
        if (ShootCB == null) {
            ShootCB = cb;
        }
        anim.SetTrigger("shoot");
    }

    public void AnimationEventShoot() {
        if (ShootCB != null) {
            ShootCB();
        }
    }
}

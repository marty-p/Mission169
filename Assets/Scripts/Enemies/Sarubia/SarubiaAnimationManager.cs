using UnityEngine;
using Utils;
using System.Collections;

public class SarubiaAnimationManager : MonoBehaviour {

    private Animator anim;
    private RetVoidTakeVoid ShootCB;
    private Vector2 pastPos;


    void Awake() {
        anim = GetComponent<Animator>();
    }

    void Start() {
        anim.SetBool("move", true);
        StartCoroutine("CheckIfMoving");
    }

    public void StartShootAnim(RetVoidTakeVoid cb) {
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

    //FIXME not the best way to achieve this ...
    private IEnumerator CheckIfMoving() {
        while (IsMoving()) {
            pastPos = transform.position;
            yield return new WaitForSeconds(0.1f);
        }
        anim.SetBool("move", false);
    }

    public bool IsMoving() {
        return pastPos.x != transform.position.x;
    }
}

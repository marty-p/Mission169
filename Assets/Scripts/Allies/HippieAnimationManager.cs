using UnityEngine;

public class HippieAnimationManager : MonoBehaviour, IObserver {

    private Animator anim;
    private test FreedCB;
    private test GiftSalutCB;
    private test WalkingCB;

    public delegate void test();

    void Awake () {
        anim = GetComponent<Animator>();
	}

    public void PlayFreeAnim(test cb) {
        anim.SetTrigger("freed");
        FreedCB = cb;
    }

    public void EndOfHippieFreedAnim() {
        if (FreedCB != null) {
            FreedCB();
        }
    }

    public void PlayWalkingAnim(test cb) {
        anim.SetBool("walking", true);
        WalkingCB = cb;
    }

    public void EndOfHippieWalkingAnim() {
        if (WalkingCB != null) {
            WalkingCB();
        }
    }

    private bool animGoin;
    public void PlayGiftAnim(test cb) {
        if (!animGoin) {
            animGoin = true;
            anim.SetTrigger("gift");
            GiftSalutCB = cb;
        }
    }
    public void EndOfHippieSalutAnim() {
        if (GiftSalutCB != null) {
            GiftSalutCB();
        }
    }

    public void Observe(SlugEvents ev) {
        if (ev == SlugEvents.Fall) {
            anim.SetBool("falling", true);
        } else if (ev == SlugEvents.HitGround) {
            anim.SetBool("falling", false);
        }
    }

}

using UnityEngine;

public class KnifeAnimationEvent : MonoBehaviour {

    private Animator topAnimator;

    void Awake() {
        topAnimator = GetComponent<Animator>();
    }

    public void AEEndOfKnifeAnim() {
        topAnimator.SetBool("knifeing", false);
    } 

    public void AEStartOfKnifeAnim() {
        topAnimator.SetBool("knifeing", true);
    }

}

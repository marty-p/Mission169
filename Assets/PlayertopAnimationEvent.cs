using UnityEngine;
using System.Collections;

public class PlayertopAnimationEvent : MonoBehaviour {

    public AnimationManager animManager;

    public void EndOfDeathAnim() {
        animManager.EndOfDeathAnim();
    }	

    public void AEGrenadeOut() {
        animManager.grenadeCB();
    }

    public void TT() {
//        print("its working");
    }
}

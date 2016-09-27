using UnityEngine;
using System.Collections;

public class CampFireBlowingSoldier : MonoBehaviour {

    public Animator fire;
    public Animator otherSoldier;


    public void AEBlow() {
        float moreOrLessBlown = UnityEngine.Random.Range(0, 1f);
        fire.SetFloat("blownIntensity", moreOrLessBlown);
        fire.SetTrigger("blow");
        if (moreOrLessBlown > 0.75f) {
            otherSoldier.SetTrigger("hot");
        }
    }

}

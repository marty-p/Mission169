using UnityEngine;

public class MummyBreath : MonoBehaviour {
    private Animator breathAnimator;

	void Awake () {
        breathAnimator = GetComponent<Animator>();	    
	}

    public void ThrowBreath() {
        breathAnimator.SetTrigger("breath");
    }
}

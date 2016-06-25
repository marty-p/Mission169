using UnityEngine;

public class MummyAnimationEvents : MonoBehaviour {

    public string victimsTag = "Player";
    private MummyBreath mummyBreath;

	void Awake () {
        mummyBreath = GetComponentInChildren<MummyBreath>(true);
	}
	
    public void AECastBadBreath() {
        mummyBreath.ThrowBreath();
    }

}

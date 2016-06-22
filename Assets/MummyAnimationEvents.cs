using UnityEngine;

public class MummyAnimationEvents : MonoBehaviour {

    private string victimsTag = "Player";
    private AreaOfEffectProjectile badBreath;

	// Use this for initialization
	void Awake () {
        badBreath = GetComponent<AreaOfEffectProjectile>();
	}
	
    public void AECastBadBreath() {
        badBreath.CastAOE(victimsTag, transform.position);
    }

}

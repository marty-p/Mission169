using UnityEngine;
using System.Collections;

public class PlayertopAnimationEvent : MonoBehaviour {

    public AnimationManager animManager;
    public BoxCollider2D collider;

    public void EndOfDeathAnim() {
        animManager.EndOfDeathAnim();
    }	

    public void AEGrenadeOut() {
        animManager.grenadeCB();
    }

    public void adaptColliderCouching() {
        collider.offset = new Vector2(0, 0.013f);
        collider.size = new Vector2(0.17f, 0.185f);
    }

    public void adaptColliderStanding() {
        collider.offset = new Vector2(0, 0.091f);
        collider.size = new Vector2(0.17f, 0.35f);
    }
}

using UnityEngine;
using System;


public class MummyAnimationEvents : MonoBehaviour {

    public string victimsTag = "Player";
    private Animator anim;
    private MummyBreath mummyBreath;
    private float pastXpos;
    private Vector3 pastDir;

	void Awake () {
        mummyBreath = GetComponentInChildren<MummyBreath>(true);
        anim = GetComponent<Animator>();
        pastXpos = transform.position.x;
        pastDir = transform.right;
	}
	
    public void AECastBadBreath() {
        mummyBreath.ThrowBreath();
    }

    public void Attack() {
        anim.SetTrigger("attack");
    }

    void Update() {
        // Stop/Start Walking
        if (Math.Truncate(pastXpos*1000)/1000 == Math.Truncate(transform.position.x*1000)/1000) {
            anim.SetBool("walking", false);
        } else {
            anim.SetBool("walking", true);
        }
        // Turn Around Anim when needed
        if (transform.right != pastDir) {
                anim.SetTrigger("turn_around");
        }

        pastXpos = transform.position.x;
        pastDir = transform.right;
    }

}

using UnityEngine;
using System.Collections;
using System;

public class SarubiaTest : MonoBehaviour, IReceiveDamage {

    public FlashUsingMaterial flashRed;

    public void OnDamageReceived(ProjectileProperties projectileProp, int newHP) {
        flashRed.FlashOnce();        
    }


	// Update is called once per frame
	void Update () {
        bool b = Input.GetKeyDown("b");
	    
        if (b) {
            GetComponent<SarubiaAttackManager>().PrimaryAttack();
        }

	}
}

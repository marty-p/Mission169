using UnityEngine;

public class BerserkerTopAnimEvents : MonoBehaviour {

    public Animator bottonAnim;

    public void AEAttackEnd() {
        gameObject.SetActive(false);
        bottonAnim.SetBool("attack", false);
    }
        
}

using UnityEngine;

public class CollectibleDef : MonoBehaviour {
    private FlashUsingMaterial flash;
    private Animator animator;

    public RuntimeAnimatorController animController;
    public int bulletCount;
    public int attackID;
    public int points;

    void Awake() {
        flash = GetComponent<FlashUsingMaterial>();
        animator = GetComponent<Animator>();
    }

    public void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player") {
            flash.FlashOnce(() => animator.SetTrigger("picked_up"));
        }
    }
}

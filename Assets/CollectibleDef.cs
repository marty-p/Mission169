using UnityEngine;

public class CollectibleDef : MonoBehaviour {
    private FlashUsingMaterial flash;
    private Animator animator;

    public RuntimeAnimatorController animController;
    public int bulletCount;
    public int attackID;
    public int points;
    public AudioClip weaponNameAudio;
    public string collectibleAnimationName;

    void Awake() {
        flash = GetComponent<FlashUsingMaterial>();
        animator = GetComponent<Animator>();
    }

    public void OnEnable() {
        animator.Play(collectibleAnimationName);
    }

    public void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player") {
            flash.FlashForOneFrame(() => animator.SetTrigger("picked_up"));
        }
    }
}

using UnityEngine;

public class HideBottomBodyPart : MonoBehaviour {

    public SpriteRenderer bottomSpriteRenderer;
    private Animator anim;

    void Start() {
        anim = GetComponent<Animator>();
    }

    public void HideBottomBody() {
        bottomSpriteRenderer.enabled = false;
    } 

    public void ShowBottomBody() {
        bottomSpriteRenderer.enabled = true;
    }

}

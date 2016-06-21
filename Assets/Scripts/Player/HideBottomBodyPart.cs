using UnityEngine;

public class HideBottomBodyPart : MonoBehaviour {

    public SpriteRenderer bottomSpriteRenderer;

    public void HideBottomBody() {
        bottomSpriteRenderer.enabled = false;
    } 

    public void ShowBottomBody() {
        bottomSpriteRenderer.enabled = true;
    }

}

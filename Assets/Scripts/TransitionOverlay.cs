using UnityEngine;
using UnityEngine.UI;
using Mission169;

public class TransitionOverlay : MonoBehaviour {

    private Image blackOverlay;

	// Use this for initialization
	void Awake () {
        blackOverlay = GetComponent<Image>();
	}

    public void SetVisible(bool visible) {
        gameObject.SetActive(visible);
    }

    public void FadeOut() {
        blackOverlay.CrossFadeAlpha(0, 0.15f, false);
    }

    public void FadeIn() {
        blackOverlay.CrossFadeAlpha(255, 0.15f, false);
    }
	
}

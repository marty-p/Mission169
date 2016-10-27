using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Mission169;

public class WhiteSplash : MonoBehaviour {

    private Image whitePanel;

	// Use this for initialization
	void Awake () {
        whitePanel = GetComponent<Image>();
	}

    void Start() {
        Invoke("TransitionOut", 3);
        UIManager.Instance.MainMenuT.SetVisible(false);
        Invoke("DisplayMainMenu", 3.5f);
    }

    void TransitionOut() {
        whitePanel.CrossFadeAlpha(0, 0.15f, false);
    }

    void DisplayMainMenu() {
        UIManager.Instance.MainMenuT.SetVisible(true);
    }
	
}

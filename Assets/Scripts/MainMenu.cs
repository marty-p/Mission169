using UnityEngine;
using UnityEngine.UI;
using Mission169;

public class MainMenu : MonoBehaviour {
    public Button start;
    public Button museum;
    public Button thanks;

    public Button facebook;
    
    void Start() {
        start.onClick.AddListener(GameManager.Instance.MissionStart);
        facebook.onClick.AddListener(FacebookManager.Instance.ShareLink);
    }    

    public void SetVisible(bool visible) {
        gameObject.SetActive(visible);
    }

}

using UnityEngine;
using UnityEngine.UI;
using Mission169;

public class MainMenu : MonoBehaviour {
    public Button start;
    public Button museum;
    public Button thanks;

    public Button facebook;
    public Button gameCenter;

    private Graphic[] graphicComponents;
    private TimeUtils timeUtils;

    private readonly float timeToFade = 0.15f;

    void Awake() {
        graphicComponents = GetComponentsInChildren<Graphic>();
        timeUtils = GetComponent<TimeUtils>();
    }

    void Start() {
        start.onClick.AddListener(GameManager.Instance.MissionStart);
        facebook.onClick.AddListener(FacebookManager.Instance.ShareLink);
        gameCenter.onClick.AddListener(GameCenterManager.ShowAchievements);
    }    

    public void SetVisible(bool visible) {
        float destAlpha = visible ? 1f : 0f;
        float origin = visible ? 0 : 1;
        for (int i=0;i<graphicComponents.Length; i++) {
            graphicComponents[i].CrossFadeAlpha(destAlpha, timeToFade, false);
        }
        timeUtils.TimeDelay(timeToFade, () => gameObject.SetActive(visible));
    }



}

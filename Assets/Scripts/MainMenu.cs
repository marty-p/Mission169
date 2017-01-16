using UnityEngine;
using UnityEngine.UI;
using Mission169;

public class MainMenu : MonoBehaviour {
    public Button start;
    public Button reset;
    public Button git;

    public Button facebook;
    public Button gameCenter;

    private Animator transitionAnimator;

    private readonly float timeToFade = 1.5f;

    void Awake() {
        transitionAnimator = GetComponent<Animator>();
    }

    void Start() {
        start.onClick.AddListener(()=> {
            GameManager.Instance.MissionInit();
            GameManager.Instance.MissionStart();
        });
        facebook.onClick.AddListener(OnFacebookPressed);
        gameCenter.onClick.AddListener(GameCenterManager.ShowAchievements);
        git.onClick.AddListener(OnGitHubPressed);
        reset.onClick.AddListener(AchievementManager.Instance.ResetAchievements);
    }    

    public void SetVisible(bool visible) {
        transitionAnimator.ResetTrigger("on_screen");
        transitionAnimator.ResetTrigger("off_screen");
        string triggerName = visible ? "on_screen" : "off_screen";
        transitionAnimator.SetTrigger(triggerName);
    }

    private void OnFacebookPressed() {
        Application.OpenURL("https://m.facebook.com/benoitpinkasfeld");
    }

    private void OnGitHubPressed() {
        Application.OpenURL("https://github.com/pinkas");
    }
}

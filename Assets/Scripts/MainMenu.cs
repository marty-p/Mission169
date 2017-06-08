using UnityEngine;
using UnityEngine.UI;
using SlugLib;

public class MainMenu : MonoBehaviour {
    public Button startButton;
    public Button resetButton;
    public Button gitButton;

    public Button facebookButton;
    public Button gameCenterButton;

    private Animator transitionAnimator;

    private readonly float timeToFade = 1.5f;

    void Awake() {
        transitionAnimator = GetComponent<Animator>();
    }

    void Start() {
        startButton.onClick.AddListener(()=> {
            EventManager.TriggerEvent(GlobalEvents.MissionStartRequest);
            SetVisible(false);
        });

        EventManager.StartListening(GlobalEvents.Home, () => {
            SetVisible(true);
        });

        facebookButton.onClick.AddListener(OnFacebookPressed);
        gameCenterButton.onClick.AddListener(GameCenterManager.ShowAchievements);
        gitButton.onClick.AddListener(OnGitHubPressed);
        resetButton.onClick.AddListener(AchievementManager.Instance.ResetAchievements);
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

using UnityEngine;
using UnityEngine.UI;
using Mission169;

public class DialogManager : MonoBehaviour {
    private GameObject currentButtonGroup;
    private Animator transitionAnimator;

    public SlugReplay replay;

    public Button pauseResumeButton;
    public Button pauseHomeButton;
    public Button pauseRetryButton;
    public Button nextLevel;

    public GameObject pauseButtons;
    public GameObject successButtons;
    public GameObject gameOverButtons;

    void Awake() {
        pauseResumeButton.onClick.AddListener(OnResumePressed);
        pauseHomeButton.onClick.AddListener(OnHomePressed);
        pauseRetryButton.onClick.AddListener(OnRetryPressed);
        nextLevel.onClick.AddListener(OnNextPressed);
        transitionAnimator = GetComponent<Animator>();
    }

    public void OnResumePressed() {
        GameManager.Instance.PauseGame(false);
        Transition(false);
    }

    public void OnHomePressed() {
        GameManager.Instance.Home();
        Transition(false);
    }

    public void OnRetryPressed() {
        GameManager.Instance.MissionRetry();
        GameManager.Instance.PauseGame(false);
        Transition(false);
    }

    public void OnNextPressed() {
        GameManager.Instance.GoNextMission();
        GameManager.Instance.PauseGame(false);
        Transition(false);
    }

    public void SetVisible(bool visible) {
        //currentButtonGroup.SetActive(false);
        Transition(visible);
    }

    public void Activate(DialogType dialogType) {
        gameObject.SetActive(true);
        if (replay != null) {
            replay.gameObject.SetActive(true);
        }

        if (dialogType == DialogType.Pause) {
            currentButtonGroup = pauseButtons;
        } else if(dialogType == DialogType.MissionSuccess) {
            currentButtonGroup = successButtons;
        } else if (dialogType == DialogType.GameOver) {
            currentButtonGroup = gameOverButtons;
        }
        currentButtonGroup.SetActive(true);
        SetVisible(true);
    }

    public void AEtransitionInDone() {
        GameManager.Instance.PauseGame(true);
    }

    public void AEtransitionOutDone() {
        SetVisible(false);
        gameObject.SetActive(false);
    }

    void Transition(bool on) {
        transitionAnimator.ResetTrigger("on_screen");
        transitionAnimator.ResetTrigger("off_screen");
        string trigger = on ? "on_screen" : "off_screen";
        transitionAnimator.SetTrigger(trigger);
    }

}

public enum DialogType {
    Pause,
    GameOver,
    MissionSuccess
}

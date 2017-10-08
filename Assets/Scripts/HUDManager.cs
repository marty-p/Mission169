using UnityEngine;
using UnityEngine.UI;
using SlugLib;
using Mission169;
using DG.Tweening;

public class HUDManager : MonoBehaviour
{
    public Text scoreGUI;
    public Text bulletCountGUI;
    public Text grenadeCountGUI;
    public Text lifeCountGUI;
    public Button pauseButton;
    public GameObject goRightReminder;
    public MissionStartLettersAnimation missionLetters;
    public GameObject onScreenControl;

    private Gradient bulletCountGradient;
    private TimeUtils timeUtils;

    private readonly int armsFontSize = 47;
    // the "∞" looks much smaller than any other glyph in the font
    private readonly int armsFontSizeWhenGun = 70; 

    void Awake()
    {
        EventManager.StartListening(GlobalEvents.GunUsed, SetBulletCount);
        EventManager.StartListening(GlobalEvents.PlayerDead, SetBulletCountToInfinity);
        EventManager.StartListening(GlobalEvents.GrenadeUsed, SetGrenadeCount);

        EventManager.StartListening(GlobalEvents.MissionStart, OnMissionStart);
        EventManager.StartListening(GlobalEvents.MissionSuccess, OnMissionSuccess);
        EventManager.StartListening(GlobalEvents.PointsEarned, OnPlayerPointsChanged);
        EventManager.StartListening(GlobalEvents.PlayerDead, OnPlayerDeath);
        EventManager.StartListening(GlobalEvents.GameOver, ()=> SetVisible(false) );
        EventManager.StartListening(GlobalEvents.Home, ()=> SetVisible(false) );

        bulletCountGradient = bulletCountGUI.GetComponent<Gradient>();
        timeUtils = GetComponent<TimeUtils>();
        pauseButton.onClick.AddListener(OnPausePressed);

#if UNITY_IOS || UNITY_ANDROID
        onScreenControl.SetActive(true);
#endif
    }

    public void SetVisible(bool visible) {
        gameObject.SetActive(visible);
    }

    public void SetLifeCount(int lifeCount) {
        lifeCountGUI.text = lifeCount.ToString();
    }

    public void SetScore(int score) {
        scoreGUI.text = score.ToString();
    }

    public void SetGrenadeCount(float grenadeCount) {
        grenadeCountGUI.text = grenadeCount.ToString();
    }

    public void SetBulletCount(float bulletCount) {
        if (bulletCount > 0) {
            bulletCountGradient.enabled = false;
            timeUtils.TimeDelay(0.05f, () => { bulletCountGradient.enabled = true; });
            bulletCountGUI.fontSize = armsFontSize;
            bulletCountGUI.text = bulletCount.ToString();
        } else {
            SetBulletCountToInfinity();
        }
    }

    public void RemindPlayerGoRight() {
        goRightReminder.SetActive(true);
    }

    public void ShowMissionStartAnimation() {
        missionLetters.SetStart();
        missionLetters.StartAnim();
    }

    void SetBulletCountToInfinity() {
        bulletCountGUI.text = "∞";
        bulletCountGUI.fontSize = armsFontSizeWhenGun;
    }

    void OnPausePressed() {
        UIManager.Instance.Dialog.Activate(DialogType.Pause);
    }

    void OnMissionStart() {
        SetVisible(true);
        ShowMissionStartAnimation();
        SetLifeCount(GameManager.PlayerLifeCount);
        SetScore(GameManager.PlayerScore);
    }

    void OnMissionSuccess()
    {
        missionLetters.SetComplete();
        missionLetters.StartAnim();
    }

    void OnPlayerPointsChanged() {
        DOVirtual.DelayedCall(0.1f, ()=> SetScore(GameManager.PlayerScore) );
    }

    void OnPlayerDeath() {
        DOVirtual.DelayedCall(0.1f, () => SetLifeCount(GameManager.PlayerLifeCount) );
    }

}

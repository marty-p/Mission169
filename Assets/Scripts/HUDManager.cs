using UnityEngine;
using UnityEngine.UI;
using SlugLib;
using Mission169;
using DG.Tweening;

public class HUDManager : MonoBehaviour {

    public Text scoreGUI;
    public Text bulletCountGUI;
    public Text grenadeCountGUI;
    public Text lifeCountGUI;
    public Button pauseButton;
    public GameObject goRightReminder;
    public GameObject readyGoPrefab;
    public GameObject onScreenControl;

    private GameObject readyGoInstance;
    private Gradient bulletCountGradient;
    private Color bulletCountGradientInitialColor;
    private TimeUtils timeUtils;

    private readonly int armsFontSize = 47;
    // the "∞" looks much smaller than any other glyph in the font
    private readonly int armsFontSizeWhenGun = 70; 

    void Awake() {
        EventManager.Instance.StartListening(GlobalEvents.GunUsed, SetBulletCount);
        EventManager.Instance.StartListening(GlobalEvents.PlayerDead, SetBulletCountToInfinity);
        EventManager.Instance.StartListening(GlobalEvents.GrenadeUsed, SetGrenadeCount);

        EventManager.Instance.StartListening(GlobalEvents.MissionStart, OnMissionStart);
        EventManager.Instance.StartListening(GlobalEvents.PointsEarned, OnPlayerPointsChanged);
        EventManager.Instance.StartListening(GlobalEvents.PlayerDead, OnPlayerDeath);
        EventManager.Instance.StartListening(GlobalEvents.GameOver, ()=> SetVisible(false) );
        EventManager.Instance.StartListening(GlobalEvents.Home, ()=> SetVisible(false) );

        bulletCountGradient = bulletCountGUI.GetComponent<Gradient>();
        bulletCountGradientInitialColor = bulletCountGradient.bottomColor;
        timeUtils = GetComponent<TimeUtils>();
        pauseButton.onClick.AddListener(OnPausePressed);
        readyGoInstance = Instantiate(readyGoPrefab);
        readyGoInstance.transform.parent = transform;
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

    public void ShowReadyGo() {
        readyGoPrefab.SetActive(true);
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
        ShowReadyGo();
        SetLifeCount(GameManager.PlayerLifeCount);
        SetScore(GameManager.PlayerScore);
    }

    void OnPlayerPointsChanged() {
        DOVirtual.DelayedCall(0.1f, ()=> SetScore(GameManager.PlayerScore) );
    }

    void OnPlayerDeath() {
        DOVirtual.DelayedCall(0.1f, () => SetLifeCount(GameManager.PlayerLifeCount) );
    }

}

using UnityEngine;
using UnityEngine.UI;
using SlugLib;

public class HUDManager : MonoBehaviour {

    public Text scoreGUI;
    public Text bulletCountGUI;
    public Text grenadeCountGUI;
    public Text lifeCountGUI;
    public GameObject goRightReminder;

    private Gradient bulletCountGradient;
    private Color bulletCountGradientColor;

    private readonly int armsFontSize = 47;
    // the "∞" looks much smaller than any other glyph in the font
    private readonly int armsFontSizeWhenGun = 70; 

    void Awake() {
        EventManager.Instance.StartListening(GlobalEvents.GunUsed, SetBulletCount);
        EventManager.Instance.StartListening(GlobalEvents.PlayerDead, SetBulletCountToInfinity);
        EventManager.Instance.StartListening(GlobalEvents.GrenadeUsed, SetGrenadeCount);
        bulletCountGradient = bulletCountGUI.GetComponent<Gradient>();
    }

    public void SetVisible(bool visible) {
        if (visible) {
            gameObject.SetActive(true);
        } else {
            gameObject.SetActive(false);
        }
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
            bulletCountGUI.fontSize = armsFontSize;
            bulletCountGUI.text = bulletCount.ToString();
        } else {
            SetBulletCountToInfinity();
        }
    }

    public void RemindPlayerGoRight() {
        goRightReminder.SetActive(true);
    }

    void SetBulletCountToInfinity() {
        bulletCountGUI.text = "∞";
        bulletCountGUI.fontSize = armsFontSizeWhenGun;
    }

}

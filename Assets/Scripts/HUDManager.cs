using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour {

    public Text scoreGUI;
    public Text bulletCountGUI;
    public Text grenadeCountGUI;
    public Text lifeCountGUI;

    void Awake() {
        EventManager.StartListening("bullet_shot", SetBulletCount);
        EventManager.StartListening("player_death", SetBulletCountToInfinity);
        EventManager.StartListening("grenade_thrown", SetGrenadeCount);
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
            bulletCountGUI.text = bulletCount.ToString();
        } else {
            SetBulletCountToInfinity();
        }
    }

    void SetBulletCountToInfinity() {
        bulletCountGUI.text = "∞";
    }

}

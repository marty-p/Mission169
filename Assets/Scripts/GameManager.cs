using UnityEngine;
using UnityEngine.UI;
using Slug;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public int playerLifeCount = 2;
    public int playerScore = 0;

    public Text scoreGUI;
    public Text bulletCountGUI;
    public Text grenadeCountGUI;
    public Text lifeCountGUI;
    // TODO have a struct or class that contains all UI stuff so that
    // I just need the HUD to be public and I'add access everythig through it
    public GameObject HUD;

    public Text pressAnyKey;

    public PlayerDeathManager player; // TODO the gameManager should instantiate the player
    // Keep it like it for now in order to always have a Player in the hierarchy even when not running
    private TimeUtils timeUtils;
    private float waitTimeBeforeSpawn = 1.5f;
    private SlugAudioManager soundManager;
    private GameObject playerGameObject;
    public Transform missionStartLocation;
    private bool missionEnded;
    private AudioSource bgMusic;

    public EnemySpawner endEnemySpwaner;
    private HealthManager endEnemyHealth;
    private bool bossStarted;
    private bool playing;

    void Awake() {
        timeUtils = GetComponent<TimeUtils>();
        EventManager.StartListening("player_death", OnplayerDeath);
        EventManager.StartListening("boss_start", BossStart);
        EventManager.StartListening("mission_end", OnMissionEnd);
        EventManager.StartListening("add_points", UpdatePlayerPoints);
        EventManager.StartListening("bullet_shot", UpdatePlayerBulletCount);
        EventManager.StartListening("grenade_thrown", UpdatePlayerGrenadeCountUI);

        soundManager = GetComponentInChildren<SlugAudioManager>();
        playerGameObject = GameObject.FindGameObjectWithTag("Player");
    }

	void Start () {
        endEnemyHealth = endEnemySpwaner.GetEnemy().GetComponent<HealthManager>();
        OnMissionLoaded();
	}
	
    void Update() {
        if (Input.anyKeyDown && !playing) {
            playing = true;
            MissionStart();
        }

        if (Input.GetButtonDown("Pause")) {
            Time.timeScale = Time.timeScale == 0 ? 1 : 0;
        }
    }

    private void OnplayerDeath() {
        playerLifeCount--;
        UpdatePlayerBulletCount(0);
        lifeCountGUI.text = playerLifeCount.ToString();
        if (playerLifeCount >= 0) {
            timeUtils.TimeDelay(waitTimeBeforeSpawn, () => {
                player.SpawnPlayer();
                EventManager.TriggerEvent("player_back_alive");
            });
        } else {
            GameOver();
        }
    }

    private void OnMissionLoaded() {
        HUDSetActive(false);
        playerGameObject.transform.position = missionStartLocation.position;
        playerGameObject.SetActive(false);
    }

    private void MissionStart() {
        lifeCountGUI.text = playerLifeCount.ToString();
        HUDSetActive(true);
        pressAnyKey.enabled = false;
        playerGameObject.SetActive(true);
        timeUtils.TimeDelay(0.1f, () => {
            bgMusic = soundManager.PlaySound(0);
            while (!bgMusic.isPlaying) {
                bgMusic = soundManager.PlaySound(0);
            }
        });
    }

    private void BossStart() {
        bossStarted = true;
        bgMusic.Stop();
        bgMusic = soundManager.PlaySound(1);
    }

    private void OnMissionEnd() {
        missionEnded = true;
        soundManager.PlaySound(2);
        soundManager.PlaySound(3);
        bgMusic.Stop();
        playerGameObject.GetComponent<MovementManager>().StopMoving();
        playerGameObject.GetComponent<InputManager>().enabled = false;
        playerGameObject.GetComponent<AnimationManager>().MissionCompleteAnim();
        playerGameObject.layer = (int) SlugLayers.IgnoreRaycast; //to ignore any potential projectile still going

        timeUtils.TimeDelay(10, () => { SceneManager.LoadScene("mainscene"); });
    }

    private void GameOver() {
        bgMusic.Stop();
        bgMusic = soundManager.PlaySound(4);
        timeUtils.TimeDelay(5, () => { SceneManager.LoadScene("mainscene"); });
    }

    private void UpdatePlayerPoints(float pts) {
        playerScore = playerScore + (int) pts;
        scoreGUI.text = playerScore.ToString();
    }

    private void UpdatePlayerBulletCount(float bullet) {
        if ( bullet > 0 ) {
            bulletCountGUI.text = bullet.ToString();
        } else {
            bulletCountGUI.text = "∞";
        }
    }

    private void UpdatePlayerGrenadeCountUI(float grenadeCount) {
        grenadeCountGUI.text = grenadeCount.ToString();
    }

    private void HUDSetActive(bool active) {
        HUD.SetActive(active);
    }
}

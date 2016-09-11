using UnityEngine;
using UnityEngine.UI;
using Slug;

public class GameManager : MonoBehaviour {

    public int playerLifeCount = 2;
    public int playerScore = 0;
    public int playerBulletCount = 0;

    public Text scoreGUI;
    public Text bulletGUI;
    private int score;


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

    void Awake() {
        timeUtils = GetComponent<TimeUtils>();
        EventManager.StartListening("player_death",
                    ()=> timeUtils.TimeDelay(waitTimeBeforeSpawn, OnplayerDeath));

        EventManager.StartListening("mission_end",
                    OnMissionEnd);

        EventManager.StartListening("add_points", UpdatePlayerPoints);
        EventManager.StartListening("bullet_shot", UpdatePlayerBulletCount);

        soundManager = GetComponentInChildren<SlugAudioManager>();
        playerGameObject = GameObject.FindGameObjectWithTag("Player");

    }

	void Start () {
        endEnemyHealth = endEnemySpwaner.GetEnemy().GetComponent<HealthManager>();
        OnMissionStart();
	}
	
    void Update() { 
        if (endEnemyHealth.gameObject.activeSelf && !bossStarted) {
            OnBossStart();
        }

        if (endEnemyHealth.currentHP < 1 && !missionEnded) {
            OnMissionEnd();
        }
    }

    private void OnplayerDeath() {
        playerLifeCount--;
        if (playerLifeCount >= 0) {
            player.SpawnPlayer();
            EventManager.TriggerEvent("player_back_alive");
        } else {
            GameOver();
        }
    }

    private void OnMissionStart() {
        bgMusic = soundManager.PlaySound(0);
        playerGameObject.transform.position = missionStartLocation.position;
    }

    private void OnBossStart() {
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
    }

    private void GameOver() {

    }

    private void UpdatePlayerPoints(float pts) {
        playerScore = playerScore + (int) pts;
        scoreGUI.text = playerScore.ToString();
    }

    private void UpdatePlayerBulletCount(float bullet) {
        if ( bullet > 0 ) {
            bulletGUI.text = bullet.ToString();
        } else {
            bulletGUI.text = "∞";
        }
    }

}

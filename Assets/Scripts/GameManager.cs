using UnityEngine;
using UnityEngine.UI;
using Slug;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public int playerLifeCount = 2;
    public int playerScore = 0;

    // TODO should the GameManager Instantiate UIManager, the player and so?
    public UIManager uiManager;

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
            TogglePauseGame();
        }
    }
    
    //TODO block user input too
    public void TogglePauseGame() {
        Time.timeScale = Time.timeScale == 0 ? 1 : 0;
    }

    private void OnplayerDeath() {
        playerLifeCount--;
        uiManager.SetLifeCount(playerLifeCount);
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
        uiManager.SetHUDActive(false);
        playerGameObject.transform.position = missionStartLocation.position;
        playerGameObject.SetActive(false);
    }

    private void MissionStart() {
        uiManager.SetLifeCount(playerLifeCount);
        uiManager.SetHUDActive(true);
        uiManager.SetMainMenuActive(false);
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
        uiManager.SetScore(playerScore);
    }

}

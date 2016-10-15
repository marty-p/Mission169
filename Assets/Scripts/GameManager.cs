using UnityEngine;
using Slug;
using UnityEngine.SceneManagement;

namespace Mission169 {

    public class GameManager : Singleton<GameManager> {

        protected GameManager() {}

        private readonly int playerLifeStart = 2;
        private int playerLifeCount;
        private int playerScore = 0;
        private bool missionEnded;
        private bool bossStarted;
        private readonly float waitTimeBeforeSpawn = 1.5f;

        private HUDManager hudManager;
        private GameObject mainMenu;
        private TimeUtils timeUtils;
        private SlugAudioManager soundManager;
        public Transform missionStartLocation;
        private AudioSource bgMusic;
        private GameObject playerGameObject;
        private PlayerDeathManager playerDeathManager;
        public GameObject PlayerPrefab; // don't want to use Resources.Load for playa
        public EnemySpawner endEnemySpwaner;
        private HealthManager endEnemyHealth;

        void Awake() {
            DontDestroyOnLoad(this);

            timeUtils = GetComponent<TimeUtils>();
            EventManager.StartListening("player_death", OnplayerDeath);
            EventManager.StartListening("boss_start", BossStart);
            EventManager.StartListening("mission_end", OnMissionEnd);
            EventManager.StartListening("add_points", UpdatePlayerPoints);
            soundManager = GetComponentInChildren<SlugAudioManager>();
            // Player
            playerGameObject = Instantiate(PlayerPrefab);
            playerDeathManager = playerGameObject.GetComponentInChildren<PlayerDeathManager>();
            playerGameObject.SetActive(false);
            // UI
            GameObject hudManagerGO = Instantiate(Resources.Load("HUD")) as GameObject;
            hudManager = hudManagerGO.GetComponent<HUDManager>();
            hudManager.SetVisible(false);
            mainMenu = Instantiate(Resources.Load("MainMenu")) as GameObject;
        }

        void Start() {
            endEnemyHealth = endEnemySpwaner.GetEnemy().GetComponent<HealthManager>();
            OnMissionLoaded();
        }

        void Update() {
            if (Input.GetButtonDown("Pause")) {
                TogglePauseGame();
            }
        }

        public GameObject GetPlayer() {
            return playerGameObject;
        }

        public void MissionStart() {
            playerScore = 0; // TODO only one mission for now so ...
            playerLifeCount = playerLifeStart;
            hudManager.SetLifeCount(playerLifeCount);
            hudManager.SetVisible(true);
            mainMenu.SetActive(false);
            playerGameObject.SetActive(true);
            bgMusic = soundManager.PlaySound(0);
        }

        //TODO block user input too
        public void TogglePauseGame() {
            Time.timeScale = Time.timeScale == 0 ? 1 : 0;
        }

        private void OnplayerDeath() {
            playerLifeCount--;
            hudManager.SetLifeCount(playerLifeCount);
            if (playerLifeCount >= 0) {
                timeUtils.TimeDelay(waitTimeBeforeSpawn, () => {
                    playerDeathManager.SpawnPlayer();
                    EventManager.TriggerEvent("player_back_alive");
                });
            } else {
                GameOver();
            }
        }

        private void OnMissionLoaded() {
            playerGameObject.transform.position = missionStartLocation.position;
            playerGameObject.SetActive(false);
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
            playerGameObject.layer = (int)SlugLayers.IgnoreRaycast; //to ignore any potential projectile still going

            timeUtils.TimeDelay(10, () => { SceneManager.LoadScene("mainscene"); });
        }

        private void GameOver() {
            bgMusic.Stop();
            bgMusic = soundManager.PlaySound(4);
            timeUtils.TimeDelay(5, () => { SceneManager.LoadScene("mainscene"); });
        }

        private void UpdatePlayerPoints(float pts) {
            playerScore = playerScore + (int)pts;
            hudManager.SetScore(playerScore);
        }

    }
}

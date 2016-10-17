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

        private TimeUtils timeUtils;
        private SlugAudioManager soundManager;
        private Transform missionStartPos;
        private AudioSource bgMusic;
        private GameObject playerGameObject;
        private PlayerDeathManager playerDeathManager;
        private HUDManager hud;
        private MainMenu mainMenu;

        public GameObject playerPrefab;

        void Awake() {
            DontDestroyOnLoad(this);

            timeUtils = gameObject.AddComponent<TimeUtils>();
            EventManager.Instance.StartListening(GlobalEvents.PlayerDead, OnplayerDeath);
            EventManager.Instance.StartListening(GlobalEvents.BossStart, OnBossStart);
            EventManager.Instance.StartListening(GlobalEvents.MissionSuccess, OnMissionSuccess);
            EventManager.Instance.StartListening(GlobalEvents.PointsEarned, UpdatePlayerPoints);
            soundManager = GetComponentInChildren<SlugAudioManager>();
            // Player
            playerGameObject = Instantiate(playerPrefab);
            playerDeathManager = playerGameObject.GetComponentInChildren<PlayerDeathManager>();
            playerGameObject.SetActive(false);
            playerGameObject.transform.parent = transform;
            hud = UIManager.Instance.HUD;
            mainMenu = UIManager.Instance.MainMenuT;
        }

        public GameObject GetPlayer() {
            return playerGameObject;
        }

        public void InitMission(Transform missionStartPos) {
            this.missionStartPos = missionStartPos;
        }

        public void MissionStart() {
            playerScore = 0; // TODO only one mission for now so ...
            playerLifeCount = playerLifeStart;
            playerGameObject.transform.position = missionStartPos.position;
            hud.SetLifeCount(playerLifeCount);
            hud.SetVisible(true);
            mainMenu.SetVisible(false);
            playerGameObject.SetActive(true);
            bgMusic = soundManager.PlaySound(0);
        }

        //TODO block user input too
        public void TogglePauseGame() {
            Time.timeScale = Time.timeScale == 0 ? 1 : 0;
        }

        public void ShowMenu() {
            mainMenu.SetVisible(true);
        }

        private void UpdatePlayerPoints(float pts) {
            playerScore = playerScore + (int)pts;
            hud.SetScore(playerScore);
        }

        private void OnplayerDeath() {
            playerLifeCount--;
            hud.SetLifeCount(playerLifeCount);
            if (playerLifeCount >= 0) {
                timeUtils.TimeDelay(waitTimeBeforeSpawn, () => {
                    playerDeathManager.SpawnPlayer();
                    EventManager.Instance.TriggerEvent("player_back_alive");
                });
            } else {
                GameOver();
            }
        }

        private void BossStart() {
            bossStarted = true;
            bgMusic.Stop();
            bgMusic = soundManager.PlaySound(1);
        }

        private void OnMissionSuccess() {
            EventManager.Instance.TriggerEvent("mission_success");
            missionEnded = true;
            soundManager.PlaySound(2);
            soundManager.PlaySound(3);
            bgMusic.Stop();
            playerGameObject.GetComponentInChildren<MovementManager>().StopMoving();
            playerGameObject.GetComponentInChildren<InputManager>().enabled = false;
            playerGameObject.GetComponentInChildren<AnimationManager>().MissionCompleteAnim();
            playerGameObject.layer = (int)SlugLayers.IgnoreRaycast; //to ignore any potential projectile still going
            MissionEnd();
        }

        private void GameOver() {
            bgMusic.Stop();
            bgMusic = soundManager.PlaySound(4);
            MissionEnd();
        }

        private void MissionEnd() {
            AchievementManager.Instance.SaveAchievementsLocally();
            timeUtils.TimeDelay(10, () => { SceneManager.LoadScene("mainscene"); });
        }

    }
}

using UnityEngine;
using Slug;
using UnityEngine.SceneManagement;
using SlugLib;

namespace Mission169 {

    public class GameManager : Singleton<GameManager> {

        protected GameManager() { }

        private static readonly int playerLifeStart = 2;
        private static int playerLifeCount = playerLifeStart;
        private int playerScore = 0;
        private bool missionEnded;
        private bool bossStarted;
        private readonly float waitTimeBeforeSpawn = 1.5f;
        private int currentMissionID = 0;
        private string[] missionList = new[] { "mission1", "mission2" };

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

        public void MissionInit() {
            MissionDef missionDef = FindObjectOfType(typeof(MissionDef)) as MissionDef; ;
            playerGameObject.GetComponentInChildren<AnimationManager>().ResetAnimators();
            playerGameObject.layer = (int)SlugLayers.Player; //to ignore any potential projectile still going
            playerGameObject.transform.GetChild(0).transform.position = missionDef.startPos.position;
            playerGameObject.SetActive(false);
            //Init music too
        }

        public void MissionStart() {
            playerGameObject.GetComponentInChildren<InputManager>().enabled = true;
            hud.SetLifeCount(playerLifeCount);
            hud.SetVisible(true);
            mainMenu.SetVisible(false);
            playerGameObject.SetActive(true);
            //bgMusic = soundManager.PlaySound(0);
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
                    EventManager.Instance.TriggerEvent(GlobalEvents.PlayerSpawned);
                });
            } else {
                GameOver();
            }
        }

        private void OnBossStart() {
            bossStarted = true;
            bgMusic.Stop();
            bgMusic = soundManager.PlaySound(1);
        }

        private void OnMissionSuccess() {
            missionEnded = true;
            //soundManager.PlaySound(2);
            //soundManager.PlaySound(3);
            //bgMusic.Stop(); 
            //TODO These should be in one function in one component of the player
            playerGameObject.GetComponentInChildren<MovementManager>().StopMoving();
            playerGameObject.GetComponentInChildren<InputManager>().enabled = false;
            playerGameObject.GetComponentInChildren<AnimationManager>().MissionCompleteAnim();
            playerGameObject.layer = (int)SlugLayers.IgnoreRaycast; //to ignore any potential projectile still going
            MissionEnd();
            currentMissionID++;
            if (currentMissionID < missionList.Length) {
                //TODO BEN create a dialog that offers to go the next level
                //UIManager.Instance.CreateSuccessDialog()
                MissionLoad();
                MissionInit();
                Invoke("MissionStart", 5);
            } else {
                //UIManager.Instance.CreateGameBeatenDialogSuperSuccess()
                currentMissionID = 0;
                MissionLoad();
                mainMenu.SetVisible(true);
            }
        }

        private void MissionLoad() {
            SceneManager.LoadScene(missionList[currentMissionID]);
            SceneManager.LoadScene("loading", LoadSceneMode.Additive);
            timeUtils.TimeDelay(2, () => SceneManager.UnloadScene("loading"));
        }

        private void GameOver() {
            //bgMusic.Stop();
            //bgMusic = soundManager.PlaySound(4);
            MissionEnd();
            playerScore = 0;
            currentMissionID = 0;
            playerLifeCount = playerLifeStart;
            //TODO BEN create a dialog that offers to retry or go to the main menu
            //UIManager.Instance.CreateGameOverDialog()
        }

        private void MissionEnd() {
            AchievementManager.Instance.SaveAchievementsLocally();
        }

        void OnMissionLoaded(Scene scene, LoadSceneMode mode) {
        }

    }
}

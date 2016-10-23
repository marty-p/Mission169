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
        private readonly float waitTimeBeforeSpawn = 1.5f;
        private int currentMissionID = 0;
        private string[] missionList = new[] { "mission1", "mission2" };

        private TimeUtils timeUtils;
        private Transform missionStartPos;
        private GameObject playerGameObject;
        private PlayerDeathManager playerDeathManager;
        private HUDManager hud;
        private MainMenu mainMenu;

        public GameObject playerPrefab;

        void Awake() {
            DontDestroyOnLoad(this);

            EventManager.Instance.StartListening(GlobalEvents.PlayerDead, OnplayerDeath);
            EventManager.Instance.StartListening(GlobalEvents.BossDead, OnMissionSuccess);
            EventManager.Instance.StartListening(GlobalEvents.PointsEarned, UpdatePlayerPoints);
            timeUtils = gameObject.AddComponent<TimeUtils>();
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
            playerGameObject.GetComponentInChildren<AnimationManager>().ResetAnimators();
            playerGameObject.layer = (int)SlugLayers.Player;
            GameObject startPos = GameObject.Find("StartLocation");
            playerGameObject.transform.GetChild(0).transform.position = startPos.transform.position;
            playerGameObject.SetActive(false);
        }

        public void MissionStart() {
            EventManager.Instance.TriggerEvent(GlobalEvents.MissionStart);
            playerGameObject.GetComponentInChildren<InputManager>().enabled = true;
            hud.SetLifeCount(playerLifeCount);
            hud.SetVisible(true);
            mainMenu.SetVisible(false);
            playerGameObject.SetActive(true);
        }

        //TODO block user input too
        public void TogglePauseGame() {
            Time.timeScale = Time.timeScale == 0 ? 1 : 0;
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

        private void OnMissionSuccess() {
            MissionEnd();
            EventManager.Instance.TriggerEvent(GlobalEvents.MissionSuccess);
            currentMissionID++;
            if (currentMissionID < missionList.Length) {
                //TODO create a dialog that offers to go the next level
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
            hud.SetVisible(false);
            SceneManager.LoadScene(missionList[currentMissionID]);
            SceneManager.LoadScene("loading", LoadSceneMode.Additive);
            timeUtils.TimeDelay(2, () => {
                SceneManager.UnloadScene("loading");
                hud.SetVisible(true);
            });
        }

        private void GameOver() {
            MissionEnd();
            EventManager.Instance.TriggerEvent(GlobalEvents.GameOver);
            playerScore = 0;
            currentMissionID = 0;
            playerLifeCount = playerLifeStart;
            hud.SetVisible(false);
            //TODO create a dialog that offers to retry or go to the main menu
            //UIManager.Instance.CreateGameOverDialog()
        }

        private void MissionEnd() {
            EventManager.Instance.TriggerEvent(GlobalEvents.MissionEnd);
            playerGameObject.GetComponentInChildren<MovementManager>().StopMoving();
            playerGameObject.GetComponentInChildren<InputManager>().enabled = false;
            playerGameObject.GetComponentInChildren<AnimationManager>().MissionCompleteAnim();
            playerGameObject.layer = (int)SlugLayers.IgnoreRaycast; //to ignore any potential projectile still going
           // AchievementManager.Instance.SaveAchievementsLocally();
        }

    }
}

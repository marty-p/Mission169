using UnityEngine;
using Slug;
using UnityEngine.SceneManagement;
using SlugLib;
using DG.Tweening;

namespace Mission169 {

    public class GameManager : Singleton<GameManager> {

        protected GameManager() { }

        private static readonly int playerLifeStart = 3;
        private static int playerLifeCount = playerLifeStart;
        private static int playerScore = 0;
        private readonly float waitTimeBeforeSpawn = 1.5f;
        private int currentMissionID = 0;
        private string[] missionList = new[] { "mission1", "mission2" };

        private Transform missionStartPos;
        private GameObject playerGameObject;
        private Transform playerTransform;
        private PlayerDeathManager playerDeathManager;
        private DialogManager dialog;

        public static int PlayerLifeCount { get {return playerLifeCount;} }
        public static int PlayerScore { get { return playerScore; } }

        public GameObject playerPrefab;

        void Awake() {
            DontDestroyOnLoad(this);

            EventManager.StartListening(GlobalEvents.PlayerDead, OnplayerDeath);
            EventManager.StartListening(GlobalEvents.BossDead, OnMissionSuccess);
            EventManager.StartListening(GlobalEvents.PointsEarned, UpdatePlayerPoints);
            EventManager.StartListening(GlobalEvents.MissionStartRequest, MissionStart);
            playerGameObject = Instantiate( Resources.Load("Player")) as GameObject;
            playerDeathManager = playerGameObject.GetComponentInChildren<PlayerDeathManager>();
            playerGameObject.SetActive(false);
            playerGameObject.transform.parent = transform;

            //FIXME  you know what
            playerTransform = playerGameObject.transform.GetChild(0).transform;

            dialog = UIManager.Instance.Dialog;
        }

        public GameObject GetPlayer() {
            return playerGameObject;
        }

        public void Home() {
            ResetGameData();
            MissionLoad();
            InitPlayer();

            EventManager.TriggerEvent(GlobalEvents.Home);
        }

        private void InitPlayer() {
            playerGameObject.GetComponentInChildren<AnimationManager>().ResetAnimators();
            playerGameObject.layer = (int)SlugLayers.Player;
            GameObject startPos = GameObject.Find("StartLocation");
            playerTransform.position = startPos.transform.position;
            playerGameObject.SetActive(false);
        }

        private void MissionStart() {
            InitPlayer();
            EventManager.TriggerEvent(GlobalEvents.MissionStart);

            DOVirtual.DelayedCall(1.8f, () => {
                playerGameObject.SetActive(true);
                playerDeathManager.SpawnPlayer();
            });
        }

        public void MissionRetry() {
            ResetGameData();
            MissionLoad();
            InitPlayer();
            // Work around for some reason the music does not start again otherwise...
            DOVirtual.DelayedCall(0.2f, ()=> MissionStart());
        }

        public void GoNextMission() {
            currentMissionID++;
            if (currentMissionID < missionList.Length) {
                MissionLoad();
                MissionStart();
            } else {
                Home();
            }
        }

        //TODO block user input too
        public void PauseGame(bool paused) {
            Time.timeScale = paused ? 0 : 1;
        }

        private void UpdatePlayerPoints(float pts) {
            playerScore = playerScore + (int)pts;
        }

        private void OnplayerDeath() {
            playerLifeCount--;
            if (playerLifeCount >= 0) {
                DOVirtual.DelayedCall(waitTimeBeforeSpawn, () => {
                    playerDeathManager.SpawnPlayer();
                    EventManager.TriggerEvent(GlobalEvents.PlayerSpawned);
                });
            } else {
                GameOver();
            }
        }

        private void OnMissionSuccess() {
            MissionEnd();
            DOVirtual.DelayedCall(3, ShowSuccessDialog);
            EventManager.TriggerEvent(GlobalEvents.MissionSuccess);
        }

        private void MissionLoad() {
            // TODO show loading indicator
            SceneManager.LoadScene(missionList[currentMissionID]);
            // TODO hide loading indicator
        }

        private void GameOver() {
            MissionEnd();
            ResetGameData();
            dialog.Activate(DialogType.GameOver);
            EventManager.TriggerEvent(GlobalEvents.GameOver);
        }

        private void ResetGameData() {
            playerScore = 0;
            currentMissionID = 0;
            playerLifeCount = playerLifeStart;
        }

        private void MissionEnd() {
            EventManager.TriggerEvent(GlobalEvents.MissionEnd);
            playerGameObject.GetComponentInChildren<MovementManager>().StopMoving();
            playerGameObject.GetComponentInChildren<InputManager>().enabled = false;
            playerGameObject.GetComponentInChildren<AnimationManager>().MissionCompleteAnim();
            playerTransform.gameObject.layer = (int)SlugLayers.IgnoreRaycast; //to ignore any potential projectile still going
        }

        private void ShowSuccessDialog() {
            dialog.Activate(DialogType.MissionSuccess);
        }

    }
}

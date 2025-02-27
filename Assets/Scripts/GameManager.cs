﻿using UnityEngine;
using Slug;
using UnityEngine.SceneManagement;
using SlugLib;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;

namespace Mission169 {

    public class GameManager : Singleton<GameManager> {

        protected GameManager() { }

        [SerializeField] SlugCamera gameplayCameraPrefab;
        private SlugCamera gameplayCamera;

        private static readonly int playerLifeStart = 3;
        private static int playerLifeCount = playerLifeStart;
        private static int playerScore = 0;
        private readonly float waitTimeBeforeSpawn = 1.5f;
        private int currentMissionID = 0;
        private string[] missionList = new[] { "mission1", "mission2" };

        private LevelData currentLevelData;
        private GameObject playerGameObject;
        private Transform playerTransform;
        private PlayerDeathManager playerDeathManager;
        private DialogManager dialog;

        public static int PlayerLifeCount { get {return playerLifeCount;} }
        public static int PlayerScore { get { return playerScore; } }

        public GameObject playerPrefab;

        void Awake()
        {
            DontDestroyOnLoad(this);

            EventManager.StartListening(GlobalEvents.PlayerDead, OnplayerDeath);
            EventManager.StartListening(GlobalEvents.BossDead, OnMissionSuccess);
            EventManager.StartListening(GlobalEvents.PointsEarned, UpdatePlayerPoints);
            EventManager.StartListening(GlobalEvents.MissionStartRequest, StartMission);

            playerGameObject = Instantiate( Resources.Load("Player")) as GameObject;
            playerDeathManager = playerGameObject.GetComponentInChildren<PlayerDeathManager>();
            playerGameObject.SetActive(false);
            playerGameObject.transform.SetParent(transform);

            gameplayCamera = Instantiate(gameplayCameraPrefab);
            gameplayCamera.transform.SetParent(transform);

            //FIXME  you know what
            playerTransform = playerGameObject.transform.GetChild(0).transform;
            if (!playerTransform)
            {
                print("FAIL");
            }

            dialog = UIManager.Instance.Dialog;
        }

        public GameObject GetPlayer() {
            //TODO if playerGameObject null we should instantiate one ... probably
            return playerGameObject;
        }

        public void Home() {
            ResetGameData();
            MissionLoad();
            InitPlayer();

            EventManager.TriggerEvent(GlobalEvents.Home);
        }

        public void MissionRetry() {
            ResetGameData();
            MissionLoad();
            InitPlayer();
            // Work around for some reason the music does not start again otherwise...
            DOVirtual.DelayedCall(0.2f, ()=> StartMission());
        }

        public void GoNextMission() {
            currentMissionID++;
            if (currentMissionID < missionList.Length) {
                StartMission();
            } else {
                Home();
            }
        }

        //TODO block user input too
        public void PauseGame(bool paused) {
            Time.timeScale = paused ? 0 : 1;
        }

        public void StartMission()
        {
            StartCoroutine(MissionStartCoroutine());
        }

        public IEnumerator MissionStartCoroutine() {
            MissionLoad();

            yield return null;

            currentLevelData = GameObject.Find("LevelData").GetComponent<LevelData>();

            EventManager.TriggerEvent(GlobalEvents.MissionStart);

            InitPlayer();
            InitCamera();

            playerGameObject.SetActive(true);
            playerDeathManager.SpawnPlayer();
        }

        private void InitPlayer() {
            playerGameObject.GetComponentInChildren<AnimationManager>().ResetAnimators();
            playerGameObject.layer = (int)SlugLayers.Player;
            playerGameObject.SetActive(false);
            playerTransform.localPosition = Vector3.zero;
            // todo set parachute mode

            Transform startPos = currentLevelData.StartLocation;
            //TODO startPos null check should be done in leveldata
            if (startPos != null)
            {
                playerGameObject.transform.position = startPos.position;
            }
            else
            {
                Debug.LogError("Could not find a starting location in the level player put in 0,0");
                playerGameObject.transform.position = Vector2.zero;
            }
        }

        private void InitCamera()
        {
            Transform[] wayPoints = currentLevelData.CameraWayPoints;

            //TODO waypoints null check should be done in leveldata 
            if (wayPoints != null)
            {
                gameplayCamera.SetWayPoints(wayPoints);
                gameplayCamera.PositionAtWayPoint(0);
            }
            else
            {
                Debug.LogError("no camera path in the level! the cam will just go straight line to the right");
            }

            gameplayCamera.SetTargetToFollow(playerTransform);
            gameplayCamera.SetParallaxBgs(currentLevelData.ParallaxBgs);
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

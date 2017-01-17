using UnityEngine;
using UnityEngine.SceneManagement;
using Mission169;
using SlugLib;
using System.Collections;

public class Main : MonoBehaviour {

    public GameObject achievementManagerPrefab;
    public GameObject gameManagerPrefab;
    public GameObject uiManagerPrefab;
    public GameObject globalAudioPoolPrefab;
    public string firstMission = "mission1";
    private TimeUtils timeUtils;

	void Awake () {
        timeUtils = GetComponent<TimeUtils>();

#if UNITY_IOS
        Application.targetFrameRate = 60;
#endif

        // Creating singletons (in the right order is better)
        EventManager eventManager = EventManager.Instance;
        Instantiate(uiManagerPrefab);
        Instantiate(achievementManagerPrefab);
        Instantiate(gameManagerPrefab);
        Instantiate(globalAudioPoolPrefab);
        FacebookManager facebookManager = FacebookManager.Instance;

        UIManager.Instance.blackOverlay.SetVisible(true);

        FacebookManager.Instance.OnFacebookInitSuccess = () => {
        //SceneManager.LoadScene("login", LoadSceneMode.Additive);
            // Facebook init is done we can load the scene in the background 
            SceneManager.LoadScene(firstMission, LoadSceneMode.Additive);

            timeUtils.TimeDelay(1, () => {
                GameManager.Instance.Home();
                UIManager.Instance.blackOverlay.FadeOut();
            });
        };
	}

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (scene.name == firstMission) {
            GameManager.Instance.Home();
            SceneManager.LoadScene("loading", LoadSceneMode.Additive);
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

}

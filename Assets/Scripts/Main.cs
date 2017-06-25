using UnityEngine;
using UnityEngine.SceneManagement;
using Mission169;
using SlugLib;
using DG.Tweening;

public class Main : MonoBehaviour {

    public GameObject achievementManagerPrefab;
    public GameObject gameManagerPrefab;
    public GameObject uiManagerPrefab;
    public GameObject globalAudioPoolPrefab;
    public string firstMission = "mission1";

	void Awake () {

#if UNITY_IOS
        Application.targetFrameRate = 60;
#endif

        // Creating singletons (in the right order is better)
        Instantiate(uiManagerPrefab);
        Instantiate(achievementManagerPrefab);
        Instantiate(gameManagerPrefab);
        Instantiate(globalAudioPoolPrefab);


        SceneManager.LoadScene("AutoGame", LoadSceneMode.Additive);

        //FacebookManager facebookManager = FacebookManager.Instance;

       // UIManager.Instance.blackOverlay.SetVisible(true);

        /*

        FacebookManager.Instance.OnFacebookInitSuccess = () => {
        //SceneManager.LoadScene("login", LoadSceneMode.Additive);
            // Facebook init is done we can load the scene in the background 
            SceneManager.LoadScene("AutoGame", LoadSceneMode.Additive);

            DOVirtual.DelayedCall(1, () => {
                GameManager.Instance.Home();
                UIManager.Instance.blackOverlay.FadeOut();
            });
        };
        */
	}

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (scene.name == firstMission) {
            GameManager.Instance.Home();
            SceneManager.LoadScene("loading", LoadSceneMode.Additive);
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

}

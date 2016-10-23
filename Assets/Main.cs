using UnityEngine;
using UnityEngine.SceneManagement;
using Mission169;
using SlugLib;

public class Main : MonoBehaviour {

    public GameObject achievementManagerPrefab;
    public GameObject gameManagerPrefab;
    public GameObject uiManagerPrefab;
    public string firstMission = "mission1";

	void Awake () {
        SceneManager.sceneLoaded += OnSceneLoaded;
        // Creating singletons (in the right order is better)
        EventManager eventManager = EventManager.Instance;
        GameObject uiManager = Instantiate(uiManagerPrefab);
        GameObject achievementManagerSingleton = Instantiate(achievementManagerPrefab);
        GameObject gameManager = Instantiate(gameManagerPrefab);
#if UNITY_IOS || UNITY_ANDROID
        FacebookManager facebook = FacebookManager.Instance;
#endif
#if UNITY_IOS
        Application.targetFrameRate = 60;
#endif
        SceneManager.LoadScene(firstMission);
	}
	
    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (scene.name == firstMission) {
            GameManager.Instance.Home();
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

}

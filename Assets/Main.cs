using UnityEngine;
using UnityEngine.SceneManagement;
using Mission169;
using SlugLib;

public class Main : MonoBehaviour {

    public GameObject achievementManagerPrefab;
    public GameObject gameManagerPrefab;
    private readonly string firstMission = "mission2";

	void Awake () {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene("loading", LoadSceneMode.Additive);
        
        // Creating singletons (in the right order is better)
        EventManager eventManager = EventManager.Instance;
        UIManager uiManager = UIManager.Instance;
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
            SceneManager.UnloadScene("loading");
            GameManager.Instance.MissionInit();
            UIManager.Instance.MainMenuT.SetVisible(true);
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

}

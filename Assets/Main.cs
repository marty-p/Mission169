using UnityEngine;
using UnityEngine.SceneManagement;
using Mission169;
using SlugLib;

public class Main : MonoBehaviour {

    public GameObject achievementManagerPrefab;
    public GameObject gameManagerPrefab;

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
        SceneManager.LoadScene("mission1");
	}
	
    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (scene.name == "mission1") {
            SceneManager.UnloadScene("loading");
            GameManager.Instance.MissionInit();
            UIManager.Instance.MainMenuT.SetVisible(true);
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

}

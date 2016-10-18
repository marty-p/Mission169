using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Disclaimer : MonoBehaviour {

    public Text disclaimer;
    public Text pleaseHaveFunYo;
    public Text pressAnyKey;
    private TimeUtils timeUtils;
    private AsyncOperation async;
    private bool loading;

    void Awake() {
        timeUtils = GetComponent<TimeUtils>();
#if UNITY_IOS || UNITY_ANDROID
        pressAnyKey.text = "Tap screen to continue";
#else
        pressAnyKey.text = "Press any key to continue";
#endif
    }

    void Update() {
        if(Input.anyKeyDown && !loading) {
            loading = true;
            StartCoroutine(LoadScene("main"));
            disclaimer.enabled = false;
            pleaseHaveFunYo.enabled = false;
            pressAnyKey.text = "Loading ...";
        }
        if (async != null) {
            print(async.progress);
        }
    }

	void Start () {
        disclaimer.CrossFadeAlpha(255, 3, false);
        timeUtils.TimeDelay(3, () => {
            pleaseHaveFunYo.CrossFadeAlpha(255, 1, false);
            timeUtils.TimeDelay(2, () => {
                pressAnyKey.CrossFadeAlpha(255, 0.5f, false);
                StartCoroutine(SlowFlash(pressAnyKey));               
            });
        });
	}

   // Could really add that to Blink
   private IEnumerator SlowFlash(Text text) {
        while (true) {
            text.color = new Color(1, 0, 0, 0);
            yield return new WaitForSeconds(0.5f);
            text.color = new Color(255, 255, 255, 255);
            yield return new WaitForSeconds(0.5f);
        }
    }

    private IEnumerator LoadScene(string sceneName) {
        async = SceneManager.LoadSceneAsync("howtoplay");
        yield return async;
    }
    /*
    void Update() {
        if (async != null) {
            print(async.progress);
        }
    }
    */
}

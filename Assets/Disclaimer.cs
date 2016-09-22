using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Disclaimer : MonoBehaviour {

    public Text disclaimer;
    public Text pleaseHaveFunYo;
    public Text pressAnyKey;
    private TimeUtils timeUtils;

    void Awake() {
        timeUtils = GetComponent<TimeUtils>();
    }

    void Update() {
        if(Input.anyKeyDown) {
            print("yo");
            SceneManager.LoadScene("MainScene");
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

}

using UnityEngine;

public class GoRight : MonoBehaviour {

    private AudioSource audio;

    void Awake() {
        print("awake");
        audio = GetComponent<AudioSource>();
        EventManager.StartListening("all_waves_over",
                () => { gameObject.SetActive(true); });
        gameObject.SetActive(false);
	}
	
 void Start() {
        print("Start");
	}


    public void PlaySound() {
        audio.Play();
    }

    public void SetInactive() {
        gameObject.SetActive(false);
    }
}

using UnityEngine;

public class GoRight : MonoBehaviour {

    private new AudioSource audio;

    void Awake() {
        audio = GetComponent<AudioSource>();
        EventManager.StartListening("all_waves_over",
                () => { gameObject.SetActive(true); });
        gameObject.SetActive(false);
	}
	
    public void PlaySound() {
        audio.Play();
    }

    public void SetInactive() {
        gameObject.SetActive(false);
    }
}

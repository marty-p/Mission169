using UnityEngine;

public class GoRightIndicator : MonoBehaviour {

    private new AudioSource audio;

    void Awake() {
        audio = GetComponent<AudioSource>();
        EventManager.Instance.StartListening("all_waves_over",
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

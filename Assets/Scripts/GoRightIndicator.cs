using UnityEngine;
using SlugLib;

public class GoRightIndicator : MonoBehaviour {

    private new AudioSource audio;

    void Awake() {
        audio = GetComponent<AudioSource>();
        gameObject.SetActive(false);
        EventManager.Instance.StartListening(GlobalEvents.WaveEventEnd, ()=>SetActive(true));
        EventManager.Instance.StartListening(GlobalEvents.PlayerInactive, ()=>SetActive(true));
	}
	
    public void PlaySound() {
        audio.Play();
    }

    private void SetActive(bool active) {
        gameObject.SetActive(active);
    }

}

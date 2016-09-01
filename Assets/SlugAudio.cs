using UnityEngine;

namespace Slug {

    public class SlugAudio : MonoBehaviour {

        private AudioSource audioSource;
        private bool soundHasStarted;

        void Awake() {
            audioSource = GetComponent<AudioSource>();
        }

        void Update() {
            if (!audioSource.isPlaying) {
               gameObject.SetActive(false);
            }
        }


    }
}

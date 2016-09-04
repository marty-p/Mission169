using UnityEngine;

public class SlugAudioManager : MonoBehaviour {

    private ObjectPoolScript audioSourcePool;
    public AudioClip[] audioClips;

    private AudioSource source;

    void Awake() {
        audioSourcePool = GetComponent<ObjectPoolScript>();
    }

    public void PlaySound(int soundIndex) {
        GameObject audioGameObject = audioSourcePool.GetPooledObject();
        AudioSource audioSource = audioGameObject.GetComponent<AudioSource>();
        audioSource.clip = audioClips[soundIndex];
        audioSource.Play();
    }

    public void PlaySoundByClip(AudioClip clip) {
        GameObject audioGameObject = audioSourcePool.GetPooledObject();
        AudioSource audioSource = audioGameObject.GetComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.Play();
    }

    public void PlaySoundSameSource(int soundIndex) {
        if (source == null) {
            GameObject audioGameObject = audioSourcePool.GetPooledObject();
            source = audioGameObject.GetComponent<AudioSource>();
            source.clip = audioClips[soundIndex];
        }
        source.Play();
    }

    //FIXME
    void Update() {
        if (source != null && !source.isPlaying) {
            source = null;
        }
    }


}

using UnityEngine;

public class SlugAudioManager : MonoBehaviour {
    
    public AudioClip[] audioClips;

    private AudioSource source;

    public void InitAudioClips(AudioClip[] audioClips) {
        this.audioClips = audioClips;
    }

    public AudioSource PlaySound(int soundIndex) {
        AudioSource audioSource = GlobalAudioPool.Instance.GetAudioSource();
        audioSource.clip = audioClips[soundIndex];
        audioSource.Play();
        return audioSource;
    }

    public void PlaySoundByClip(AudioClip clip) {
        AudioSource audioSource = GlobalAudioPool.Instance.GetAudioSource();
        audioSource.clip = clip;
        audioSource.Play();
    }

    public void PlaySoundSameSource(int soundIndex) {
        if (source == null) {
            source = GlobalAudioPool.Instance.GetAudioSource();
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

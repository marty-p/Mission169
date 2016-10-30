using UnityEngine;

public class GlobalAudioPool : Singleton<GlobalAudioPool> {

    public GameObject soundPrefab;
    private ObjectPoolScript audioSourcePool;

	void Awake () {
        DontDestroyOnLoad(this);

	    audioSourcePool = gameObject.AddComponent<ObjectPoolScript>();
        audioSourcePool.pooledObject = soundPrefab;
        audioSourcePool.pooledAmount = 10;
        audioSourcePool.willGrow = true;
	}

    public AudioSource GetAudioSource() {
        return audioSourcePool.GetPooledObject().GetComponent<AudioSource>();
    }	

}

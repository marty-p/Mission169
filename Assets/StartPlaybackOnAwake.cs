using UnityEngine;
using SlugLib;

public class StartPlaybackOnAwake : MonoBehaviour {

	void Start () {
        GetComponent<RecorderMaster>().StartPlayback("rec");
	}

    void Update()
    {
        if (Input.anyKeyDown)
        {
            EventManager.TriggerEvent(GlobalEvents.MissionStartRequest);
        }
    }
}

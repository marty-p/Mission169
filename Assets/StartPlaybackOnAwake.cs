using UnityEngine;
using System.Collections;
using SlugLib;

public class StartPlaybackOnAwake : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<RecorderMaster>().StartPlaybackV2("game_play_recording");
	}



    void Update()
    {
        if (Input.anyKeyDown)
        {
            EventManager.TriggerEvent(GlobalEvents.MissionStartRequest);
        }
    }
}

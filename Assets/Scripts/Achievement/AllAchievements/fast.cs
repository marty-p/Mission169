using SlugLib;
using UnityEngine;

public class fast : Achievement {
    readonly int timeToBeatMission1 = 90;
    float startTime;

    void Start() {
        EventManager.Instance.StartListening(GlobalEvents.MissionSuccess, OnMissionSuccess);
        EventManager.Instance.StartListening(GlobalEvents.MissionStart, ()=> {startTime = Time.time;});
    }

    private void OnMissionSuccess() {
        if (Time.time - startTime <= timeToBeatMission1){
            GrantAchievement();
        }
    }
}

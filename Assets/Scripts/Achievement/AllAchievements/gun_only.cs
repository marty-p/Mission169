using SlugLib;
using UnityEngine;

// Do not slice anyone!
public class gun_only : Achievement {
    bool knifeUsed;

    void Start() {
        EventManager.StartListening(GlobalEvents.MissionStart, ()=> { knifeUsed = false;});
        EventManager.StartListening(GlobalEvents.KnifeUsed, ()=> { knifeUsed = true;});
        EventManager.StartListening(GlobalEvents.MissionSuccess, OnMissionSuccess);
    }

    private void OnMissionSuccess() {
        if (!knifeUsed) {
            GrantAchievement();
        }
    }
}

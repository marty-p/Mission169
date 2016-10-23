using SlugLib;
using UnityEngine;

// Do not slice anyone!
public class gun_only : Achievement {
    bool knifeUsed;

    void Start() {
        EventManager.Instance.StartListening(GlobalEvents.MissionStart, ()=> { knifeUsed = false;});
        EventManager.Instance.StartListening(GlobalEvents.KnifeUsed, ()=> { knifeUsed = true;});
        EventManager.Instance.StartListening(GlobalEvents.MissionSuccess, OnMissionSuccess);
    }

    private void OnMissionSuccess() {
        if (!knifeUsed) {
            GrantAchievement();
        }
    }
}

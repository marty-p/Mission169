using SlugLib;
using UnityEngine;

// Do not die for a mission
public class no_death : Achievement {
    bool died;

    void Start() {
        EventManager.Instance.StartListening(GlobalEvents.MissionStart, ()=> { died = false;} );
        EventManager.Instance.StartListening(GlobalEvents.MissionSuccess, OnMissionSuccess);
        EventManager.Instance.StartListening(GlobalEvents.PlayerDead, OnPlayerDeath);
    }

    private void OnPlayerDeath() {
        died = true;
    }

    private void OnMissionSuccess() {
        if (!died) {
            GrantAchievement();
        }
    }
}
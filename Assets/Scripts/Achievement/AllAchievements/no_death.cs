using SlugLib;
using UnityEngine;

// Do not die for a mission
public class no_death : Achievement {
    bool died;

    void Start() {
        EventManager.StartListening(GlobalEvents.MissionStart, ()=> { died = false;} );
        EventManager.StartListening(GlobalEvents.MissionSuccess, OnMissionSuccess);
        EventManager.StartListening(GlobalEvents.PlayerDead, OnPlayerDeath);
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
using SlugLib;
using UnityEngine;

// Do not pick up any special weapon
public class no_mg : Achievement {
    bool pickedUpWeapon;

    void Start() {
        EventManager.StartListening(GlobalEvents.MissionStart, ()=> { pickedUpWeapon = false;} );
        EventManager.StartListening(GlobalEvents.ItemPickedUp, ()=> { pickedUpWeapon = true;} );
        EventManager.StartListening(GlobalEvents.MissionSuccess, OnMissionSuccess);
    }

    private void OnMissionSuccess() {
        if (!pickedUpWeapon) {
            GrantAchievement();
        }
    }
}

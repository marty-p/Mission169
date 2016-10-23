using SlugLib;
using UnityEngine;

// Do not pick up any special weapon
public class no_mg : Achievement {
    bool pickedUpWeapon;

    void Start() {
        EventManager.Instance.StartListening(GlobalEvents.MissionStart, ()=> { pickedUpWeapon = false;} );
        EventManager.Instance.StartListening(GlobalEvents.ItemPickedUp, ()=> { pickedUpWeapon = true;} );
        EventManager.Instance.StartListening(GlobalEvents.MissionSuccess, OnMissionSuccess);
    }

    private void OnMissionSuccess() {
        if (!pickedUpWeapon) {
            GrantAchievement();
        }
    }
}

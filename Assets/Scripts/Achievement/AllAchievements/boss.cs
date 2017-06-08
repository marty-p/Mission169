using SlugLib;
using UnityEngine;

// Defeat the 3 towers
public class boss : Achievement {
    void Start() {
        EventManager.StartListening(GlobalEvents.BossDead, GrantAchievement);
    }
}

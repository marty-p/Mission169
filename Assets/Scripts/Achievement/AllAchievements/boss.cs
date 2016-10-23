using SlugLib;
using UnityEngine;

// Defeat the 3 towers
public class boss : Achievement {
    void Start() {
        EventManager.Instance.StartListening(GlobalEvents.BossDead, GrantAchievement);
    }
}

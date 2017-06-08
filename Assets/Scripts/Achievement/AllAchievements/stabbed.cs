using SlugLib;
using UnityEngine;

// Get stabbed
public class stabbed : Achievement {
    void Start() {
        EventManager.StartListening(GlobalEvents.PlayerStabbed, GrantAchievement);
    }
}

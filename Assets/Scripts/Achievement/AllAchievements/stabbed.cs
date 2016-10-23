using SlugLib;
using UnityEngine;

// Get stabbed
public class stabbed : Achievement {
    void Start() {
        EventManager.Instance.StartListening(GlobalEvents.PlayerStabbed, GrantAchievement);
    }
}

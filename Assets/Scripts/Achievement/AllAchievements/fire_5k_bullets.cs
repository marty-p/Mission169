using SlugLib;
using UnityEngine;

public class fire_5k_bullets : Achievement {
    readonly int globalBulletsToGrant = 5000;
    int bulletFiredToDate;

    void Start() {
        EventManager.StartListening(GlobalEvents.MissionStart, GetProgressFromServer);
        EventManager.StartListening(GlobalEvents.MissionEnd, NotifyAchievementManager);
        EventManager.StartListening(GlobalEvents.GunUsed, IncrementBulletCount);
    }

    void IncrementBulletCount(float bullet){
        Debug.Log("incrementing bulle count");
        progress +=  100f / globalBulletsToGrant;
        if (progress >= 100) {
            GrantAchievement();
        }
    }

}

using SlugLib;
using UnityEngine;

public class fire_5k_bullets : Achievement {
    readonly int globalBulletsToGrant = 5000;
    int bulletFiredToDate;

    void Start() {
        EventManager.Instance.StartListening(GlobalEvents.MissionStart, GetProgressFromServer);
        EventManager.Instance.StartListening(GlobalEvents.MissionEnd, NotifyAchievementManager);
        EventManager.Instance.StartListening(GlobalEvents.GunUsed, IncrementBulletCount);
    }

    void IncrementBulletCount(float bullet){
        Debug.Log("incrementing bulle count");
        progress +=  100f / globalBulletsToGrant;
        if (progress >= 100) {
            GrantAchievement();
        }
    }

}

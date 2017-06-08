using SlugLib;
using UnityEngine;

public class knife : Achievement {
    readonly int soldierToKnife = 10;

    void Start() {
        EventManager.StartListening(GlobalEvents.KnifeUsed, IncrementDeadSolider);
        EventManager.StartListening(GlobalEvents.MissionStart, ()=> {progress = 0;});
    }

    private void IncrementDeadSolider() {
        progress += 100 / soldierToKnife;
        if (progress >= 100) {
            NotifyAchievementManager();
        }
    }

}

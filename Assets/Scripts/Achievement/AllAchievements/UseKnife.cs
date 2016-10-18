using SlugLib;
using UnityEngine;

namespace AchievementsList {
    public class UseKnife : Achievement {
        int deadSoldier;

        void Start() {
            EventManager.Instance.StartListening(GlobalEvents.KnifeUsed, IncrementDeadSolider);
        }

        private void IncrementDeadSolider() {
            deadSoldier++;
        }

    }
}

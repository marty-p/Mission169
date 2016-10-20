using SlugLib;
using UnityEngine;

    public class Ach_knife : Achievement {
        readonly int soldierToKnife = 2;

        void Start() {
            EventManager.Instance.StartListening(GlobalEvents.KnifeUsed, IncrementDeadSolider);
        }

        private void IncrementDeadSolider() {
            progress += 100 / soldierToKnife;
            if (progress >= 100) {
                NotifyAchievementManager();
            }
        }

}

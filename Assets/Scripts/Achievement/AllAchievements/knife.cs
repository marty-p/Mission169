using SlugLib;
using UnityEngine;

    public class knife : Achievement {
        readonly int soldierToKnife = 5;

        void Start() {
            EventManager.Instance.StartListening(GlobalEvents.KnifeUsed, IncrementDeadSolider);
        }

        private void IncrementDeadSolider() {
		Debug.Log (" ---------------> One SOldier sliced");
            progress += 100 / soldierToKnife;
            if (progress >= 100) {
                NotifyAchievementManager();
            }
        }

}

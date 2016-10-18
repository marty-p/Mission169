using SlugLib;

namespace AchievementsList {
    public class KillTenSoldiers : Achievement {

        readonly int soldiersToKill = 10;
        int deadSoldier;

        void Start() {
            EventManager.Instance.StartListening(GlobalEvents.SoldierDead, IncrementDeadSolider);
        }

        private void IncrementDeadSolider() {
            deadSoldier++;
            if(deadSoldier == soldiersToKill) {
                meets_conditions = true;
            }
        }

    }
}

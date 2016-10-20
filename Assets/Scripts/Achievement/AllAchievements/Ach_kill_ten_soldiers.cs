using SlugLib;

public class Ach_kill_ten_soldiers : Achievement {

    readonly int soldiersToKill = 10;

    void Start() {
        EventManager.Instance.StartListening(GlobalEvents.SoldierDead, IncrementDeadSolider);
    }

    private void IncrementDeadSolider() {
    }

}

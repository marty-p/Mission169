using SlugLib;

public class very_first : Achievement {

    void Start() {
        EventManager.Instance.StartListening(GlobalEvents.SoldierDead, OnSoldierDead);
    }

    private void OnSoldierDead() {
        progress = 100;
        NotifyAchievementManager();
    }

}

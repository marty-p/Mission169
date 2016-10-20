using UnityEngine;

public abstract class Achievement : MonoBehaviour {
    public string ID;
    public bool dirty;
    public float progress;

    public void NotifyAchievementManager() {
        AchievementManager.Instance.UpdateAchievementProgress(this);
    }
}

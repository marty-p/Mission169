using UnityEngine;

public abstract class Achievement : MonoBehaviour {
    private string id;
    public string ID {get {return id;}}
    protected bool dirty;
    public bool Dirty {get {return dirty;}}
    protected float progress;
    public float Progress { get{return progress;} set{progress = value;} }
    protected bool granted;
    public bool Granted { get {return granted;} set {granted = value;} }

    public void Init(string id, float progress){
        this.id = id;
        this.progress = progress;
    }
    public void GrantAchievement(){
        progress = 100;
        NotifyAchievementManager();
    }

    public void GetProgressFromServer(){
        AchievementManager.Instance.RetrieveAchievementProgress(this);
    }

    public void NotifyAchievementManager() {
        AchievementManager.Instance.UpdateAchievementProgress(this);
    }

}

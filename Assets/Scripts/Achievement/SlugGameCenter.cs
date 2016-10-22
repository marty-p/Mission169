using UnityEngine;
using SA.Common.Models;

public class SlugGameCenter : MonoBehaviour, IGameService {

    void Start () {
        GameCenterManager.OnAuthFinished += OnAuthFinished;
        GameCenterManager.OnAchievementsLoaded += OnAchievementsLoaded;
		GameCenterManager.OnAchievementsReset += OnAchievementsLoaded;
		GameCenterManager.OnAchievementsProgress += OnAchievementsProgress;

        GameCenterManager.Init();
    }

	public void Reset(){
		GameCenterManager.ResetAchievements();
	}

	public void UpdateAchievement(Achievement ach){
        GameCenterManager.SubmitAchievement(ach.Progress, ach.ID, true);
	}

	public void ShowErrorDialog() {
		IOSNativePopUpManager.showMessage("Game Center Error", "You don't seem to be connected to Game Center");		
	}

    void OnAuthFinished(Result res) {
        if (res.IsSucceeded) {
			GameCenterManager.LoadAchievements();
        } else {
            Debug.LogWarning("Failed to connect to GameCenter");
        }
    }

    void OnAchievementsLoaded(Result res) {
		if (res.IsSucceeded) {
			Debug.Log ("Achievemnts were loaded from Game Center");
			foreach (GK_AchievementTemplate tpl in GameCenterManager.Achievements) {
				Debug.Log ("  ===> Achievement Loaded " + tpl.Id + " progress: " + tpl.Progress);
				if (tpl.Progress < 100) {
					AchievementManager.Instance.InstantiateAchievement (tpl.Id, tpl.Progress);
				}
			}
		} else {
			Debug.Log ("FAILED TO LOAD ACHIEVEMENTS FROM GC");
		}
    }
	void OnAchievementsProgress(Result res) {
		Debug.Log("achievemnts progress");
	}

}

using UnityEngine;

public class SlugGameCenter : MonoBehaviour, IGameService {

    void Start () {
        /*
        GameCenterManager.OnAuthFinished += OnAuthFinished;
        GameCenterManager.OnAchievementsLoaded += OnAchievementsLoaded;
        GameCenterManager.OnAchievementsReset += OnAchievementsReset;
		GameCenterManager.OnAchievementsProgress += OnAchievementsProgress;

        GameCenterManager.Init();
        */
    }

	public void Reset(){
		//GameCenterManager.ResetAchievements();
	}

	public void UpdateAchievement(Achievement ach){
        //GameCenterManager.SubmitAchievement(ach.Progress, ach.ID, true);
	}

	public void RetrieveProgress(Achievement ach) {
        //ach.Progress = GameCenterManager.GetAchievementProgress(ach.ID);
	}

	public void ShowErrorDialog() {
		//IOSNativePopUpManager.showMessage("Game Center Error", "You don't seem to be connected to Game Center");		
	}
/*
    void OnAuthFinished(Result res) {
        if (res.IsSucceeded) {
			//GameCenterManager.LoadAchievements();
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
            IOSNativePopUpManager.showMessage("Game Center Error", "Couldn't load achievements");	
        }
    }

    void OnAchievementsReset(Result res) {
        if (res.IsSucceeded) {
            OnAchievementsLoaded(res);
		    IOSNativePopUpManager.showMessage("Progress Reset", "All achievements back to 0%!");		
        } else {
		    IOSNativePopUpManager.showMessage("Progress Reset", "Couldn't reset achievements");		
        }
    }

	void OnAchievementsProgress(Result res) {
		Debug.Log("achievemnts progress");
	}
*/
}

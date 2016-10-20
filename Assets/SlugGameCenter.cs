using UnityEngine;
using SA.Common.Models;

public class SlugGameCenter : MonoBehaviour {

    void Start () {
        GameCenterManager.Init();
        GameCenterManager.OnAuthFinished += OnAuthFinished;
        GameCenterManager.OnAchievementsLoaded += OnAchievementsLoaded;
    }

    void OnAuthFinished(Result res) {
        if (res.IsSucceeded) {
            IOSNativePopUpManager.showMessage("Player Authored ",
                    "ID: " + GameCenterManager.Player.Id + "\n" + "Alias: " + GameCenterManager.Player.Alias);
        } else {
            IOSNativePopUpManager.showMessage("Game Center ", "Player auth failed");
        }
    }

    void OnAchievementsLoaded(Result res) {
        if (res.IsSucceeded) {
            Debug.Log("Achievemnts was loaded from IOS Game Center");
            foreach (GK_AchievementTemplate tpl in GameCenterManager.Achievements) {
                Debug.Log(tpl.Id + ":  " + tpl.Progress);
                Debug.Log(tpl.Id + ":  " + tpl.Title);
                Debug.Log(tpl.Id + ":  " + tpl.Description);
                if (tpl.Progress < 100) {
//                    AchievementManager.Instance
                }
            }
        }
    }

}

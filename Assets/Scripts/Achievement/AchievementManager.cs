using UnityEngine;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Collections.Generic;
using System;

public class AchievementManager : Singleton<AchievementManager> {

    protected AchievementManager() { }

    private List<Achievement> achievements = new List<Achievement>();
    private SlugGameCenter gameCenter;
    //private SlugGooglePlay googlePlayGame;

    // have a default list which is with the app (resources folder)
    // have a dynamic list with everything (what's done what's not done)
    // when it start if no dynamic we create it from default
    // when it starts IF dynamic present we still go through default to check if there is no new achievement added if so we add them to dynamic


    // check if a file exist
    // add a json object to an existing list in a precise file
    // create a file
    // update precise field from pecise object in precise file

    void Awake() {
        DontDestroyOnLoad(this);

#if UNITY_IOS
         gameCenter = gameObject.AddComponent<SlugGameCenter>();
#elif UNITY_ANDROID
        
#else
#endif
    }

/* void Awake() {
        DontDestroyOnLoad(this);

        InstantiateAllAchievements();

        // Retrieving local achievements list
        if (FirstGameLaunch()) {
            AchievementFileOperation.CreateUserFile();
            SaveAchievementsLocally();
        } else {
            ExtractLocalAchievements();
        }
#if UNITY_IOS
#elif UNITY_ANDROID
#else
#endif
    }
    */

    void Start() {
    //    InvokeRepeating("CheckUngrantedAchievements", 1, 1);
    //    InvokeRepeating("SyncWithServer", 1, 1);
    }

    public void UpdateAchievementProgress(Achievement ach) {
        GameCenterManager.SubmitAchievement(ach.progress, ach.ID);
    }

    public void InstantiateAchievement(string id, float progress) {
        Type type = Type.GetType(id);
        if (type == null) {
            Debug.LogWarning("Failed to instantiate achievement id: " + id);
        } else {
            Achievement ach = gameObject.AddComponent(type) as Achievement;
            ach.progress = progress;
            achievements.Add(ach);
        }
    }


/*
    public bool SaveAchievementsLocally() {
        //TODO exception so that you return false when it fails ...
        AchievementFileOperation.AchievementsToFile(AchievementScripts);
        return true;
    }

    void CheckUngrantedAchievements() {
        for (int i=0; i<AchievementScripts.Length; i++) {
            if (!AchievementScripts[i].granted && AchievementScripts[i].MeetsCondition) {
                print(" achievement granted! " + AchievementScripts[i].myID);
                AchievementScripts[i].granted = true;

            }
        }
    }

    // go through our list of achievement of dirty achievement
    public void SyncWithServer() {

    }

    public void AddAchievement(Achievement achievement) {
        achieves.Add(achievement);
    }

    //go through all the ones that have a progress 
    void UpdateProgressAchievements() {

    }

    bool FirstGameLaunch() {
        return !AchievementFileOperation.UserFileExists();
    }

    void ExtractLocalAchievements() {
        List <JObject> jobjList = AchievementFileOperation.ReadUserFile();
        // yes it's n^2 I know 
        for (int i = 0; i < jobjList.Count; i++) {
            int id = AchievementFileOperation.GetValueFromJson<int>(jobjList[i], "id");
            float progress = AchievementFileOperation.GetValueFromJson<float>(jobjList[i], "progress");
            bool granted = AchievementFileOperation.GetValueFromJson<bool>(jobjList[i], "meets_conditions");
            for (int j = 0; j < AchievementScripts.Length; j++) {
                if (granted) {
                    Destroy(AchievementScripts[i]);
                } else if (progress >= 0) {
                    AchievementScripts[i].progress = progress;
                }
            }
        }
    }
    */
}


public static class AchievementFileOperation {
    private static string dirPath = Application.dataPath + "/Scripts/Achievement/";
    private static string achievementDirPath = dirPath + "AllAchievements/";
    private static string originalListpath = dirPath + "original_achievement_list.json";
    private static string userListPath = Application.persistentDataPath + "/user_achievement_list.json";

    public static bool UserFileExists() {
        //TODO catch exception
        return File.Exists(userListPath);
    }

    public static void CreateUserFile() {
        // TODO catch exception 
        File.Create(userListPath).Dispose();
        Debug.Log(userListPath);
    }

    public static void AchievementsToFile(Achievement[] a) {
        string serialized = BuildJson(a);
        WriteToUserFile(serialized);
        JObject achi = JObject.Parse(File.ReadAllText(userListPath));
        achi.ToString();
    }

    static void WriteToUserFile(string str) {
        // TODO exception
        File.WriteAllText(userListPath, str);
    }

    public static List<JObject> ReadUserFile() {
        List<JObject> jobjList = new List<JObject>();
        JObject achi = JObject.Parse(File.ReadAllText(userListPath));
        JArray array = achi.Value<JArray>("achievements");
        foreach (var item in array.Children()) {
            jobjList.Add((JObject) item);
        }
        return jobjList;
    }

    public static T GetValueFromJson<T>(JObject jobject, string key) {
        var val = jobject.Value<T>(key);
        return (T) Convert.ChangeType(val, typeof(T));
    }

    static string BuildJson(Achievement[] a) {
        string serializedArray = "{ \"achievements\" : [";
        for (int i = 0; i < a.Length; i++) {
            serializedArray = serializedArray + JsonUtility.ToJson(a[i], true);
            if (i + 1 < a.Length) {
                serializedArray = serializedArray + ",";
            }
        }
        serializedArray = serializedArray + "] }";
        return serializedArray;
    }

    static void JsonToAchievements() {
        JObject achi = JObject.Parse(File.ReadAllText(originalListpath));
        JArray array = achi.Value<JArray>("achievements");
    }


}


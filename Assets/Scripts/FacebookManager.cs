using UnityEngine;
using Facebook.Unity;
using System.Collections.Generic;
using System;

public class FacebookManager : Singleton<FacebookManager> {

    void Awake() {
        DontDestroyOnLoad(this);
        if (!FB.IsInitialized) {
            FB.Init(InitCallback, OnHideUnity);
        } else {
            FB.ActivateApp();
        }
    }

    private void InitCallback() {
        if (FB.IsInitialized) {
            FB.ActivateApp();
            var perms = new List<string>() { "public_profile"};
            //FB.LogInWithReadPermissions(perms, AuthCallback);
            EventManager.Instance.StartListening(
                    "mission_success",
                    () => FB.LogAppEvent(AppEventName.AchievedLevel));
           
            EventManager.Instance.StartListening(
                    "player_death",
                    () => FB.LogAppEvent("death"));
        } else {
            Debug.Log("Failed to Initialize the Facebook SDK");
        }
    }

    private void OnHideUnity(bool isGameShown) {
        if (!isGameShown) {
            Time.timeScale = 0;
        } else {
            Time.timeScale = 1;
        }
    }

    private void AuthCallback(ILoginResult result) {
        if (FB.IsLoggedIn) {
            // AccessToken class will have session details
            var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;
            // Print current access token's User ID
            Debug.Log(aToken.UserId);
            // Print current access token's granted permissions
            foreach (string perm in aToken.Permissions) {
                Debug.Log(perm);
            }
        } else {
            Debug.Log("User cancelled login");
        }
    }


    private void ShareCallback(IShareResult result) {
        if (result.Cancelled || !String.IsNullOrEmpty(result.Error)) {
            Debug.Log("ShareLink Error: " + result.Error);
        } else if (!String.IsNullOrEmpty(result.PostId)) {
            Debug.Log(result.PostId);
        } else {
            Debug.Log("ShareLink success!");
        }
    }

    public void ShareLink() {
        FB.ShareLink(
                new Uri("https://developers.facebook.com/"),
                callback: ShareCallback
        );
    }

}

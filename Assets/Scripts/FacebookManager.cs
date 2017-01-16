using UnityEngine;
using UnityEngine.Events;
using Facebook.Unity;
using System.Collections.Generic;
using System;
using SlugLib;
using Utils;

public class FacebookManager : Singleton<FacebookManager> {
    public RetVoidTakeVoid OnFacebookInitSuccess;

    void Awake() {
        DontDestroyOnLoad(this);
        if (!FB.IsInitialized) {
            FB.Init(InitCallback, OnHideUnity);
        } else {
            FB.ActivateApp();
        }
    }

    public void Login() {
        var perms = new List<string>() {"email"};
        FB.LogInWithReadPermissions(perms, AuthCallback);
    }

    private void InitCallback() {
        if (FB.IsInitialized) {
            FB.ActivateApp();

            EventManager.Instance.StartListening(
                    GlobalEvents.MissionSuccess,
                    () => FB.LogAppEvent(AppEventName.AchievedLevel));
           
            EventManager.Instance.StartListening(
                    GlobalEvents.PlayerDead,
                    () => FB.LogAppEvent("death"));
            if (OnFacebookInitSuccess != null) {
                OnFacebookInitSuccess();
            }
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
                new Uri("https://github.com/pinkas/Mission169"),
                callback: ShareCallback
        );
    }

}

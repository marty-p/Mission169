using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Mission169;
using SlugLib;

public class DebugButtonEvents : MonoBehaviour {

    void Awake() {
        DontDestroyOnLoad(this);
    }

    void OnGUI() {
        if (GUI.Button(new Rect(10, 10, 150, 30), "Mission Success"))
            EventManager.Instance.TriggerEvent(GlobalEvents.MissionSuccess);

        if (GUI.Button(new Rect(10, 40, 150, 30), "GameOver"))
            EventManager.Instance.TriggerEvent(GlobalEvents.GameOver);

    }


}

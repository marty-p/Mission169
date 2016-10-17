using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Mission169;

public class DebugButtonEvents : MonoBehaviour {

    void Awake() {
        DontDestroyOnLoad(this);
    }

    void OnGUI() {
        if (GUI.Button(new Rect(10, 10, 100, 40), "mission_success"))
            EventManager.Instance.TriggerEvent("mission_success");

        if (GUI.Button(new Rect(10, 70, 50, 30), "Click"))
            EventManager.Instance.TriggerEvent("mission_success");

    }


}

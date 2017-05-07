using UnityEngine;

public class Cooldown {

    float duration;
    float startTime;

    public Cooldown(float duration) {
        this.duration = duration;
    }

    public void Start() {
        startTime = Time.realtimeSinceStartup;
    }

    public bool IsReady() {
        return Time.realtimeSinceStartup >= startTime + duration;
    }
}

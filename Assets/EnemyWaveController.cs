using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class EnemyWaveController : MonoBehaviour {

    public FollowTarget cam;
    private List<Transform> waves = new List<Transform>();
    private int currentWaveIndex;
    private GameObject currentWaveGameObject;
    private TimeUtils timeUtils;
    EnemySpawner[][] spawners;
    private const float updatePeriod = 0.1f;
    public bool missionEndWave;

	void Awake () {
        foreach (Transform child in transform) {
            waves.Add(child);
        }

        spawners = new EnemySpawner[waves.Count][];
        int waveIndex = 0;
        foreach (Transform child in waves) {
            spawners[waveIndex] = child.GetComponentsInChildren<EnemySpawner>();
            waveIndex++;
        }
    }
	
    private void OnTriggerEnter2D(Collider2D collider) {
        if (collider.tag == "Player") {
            timeUtils = GetComponent<TimeUtils>();

            currentWaveIndex = 0;
            currentWaveGameObject = waves[currentWaveIndex].gameObject;

            this.enabled = true;
            StartCoroutine(WaveUpdate());

            if (cam != null) {
                cam.followActive = false;
            }
        }
    }

    private IEnumerator WaveUpdate() {
        while (this.enabled) {
            if (!currentWaveGameObject.activeSelf) {
                currentWaveGameObject.SetActive(true);
            } else {
                bool currentWaveOver = true;
                for (int i = 0; i < spawners[currentWaveIndex].Length; i++) {
                    if (spawners[currentWaveIndex][i].EnemyAlive()) {
                        currentWaveOver = false; 
                    }
                }

                if (currentWaveOver) {
                    if (currentWaveIndex == waves.Count - 1) {
                        AllWavesOver();
                    } else {
                        currentWaveIndex++;
                        currentWaveGameObject = waves[currentWaveIndex].gameObject;
                    }
                }
            }

            yield return new WaitForSeconds(updatePeriod);
        }
    }

    private void AllWavesOver() {
        cam.followActive = true;
        gameObject.SetActive(false);
        EventManager.TriggerEvent("all_waves_over");
        if (missionEndWave) {
            EventManager.TriggerEvent("mission_end");
        }
    }

}

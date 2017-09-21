using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using SlugLib;
using DG.Tweening;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Mission169
{
   [ExecuteInEditMode]
    public class EnemyWaveController : MonoBehaviour
    {
        public FollowTarget cam;
        private List<Transform> waves = new List<Transform>();
        private int currentWaveIndex;
        private GameObject currentWaveGameObject;
        EnemySpawner[][] spawners;
        private const float updatePeriod = 0.1f;
        // Killing the masterEnemy is enough to end the whole wave
        public EnemySpawner masterEnemySpawner;
        private HealthManager masterEnemyHealth;

        void Awake()
        {
            // getting a reference of all the waves in the event
            foreach (Transform child in transform)
            {
                waves.Add(child);
            }

            // getting a reference of all the spawners in each wave
            spawners = new EnemySpawner[waves.Count][];
            int waveIndex = 0;
            foreach (Transform child in waves)
            {
                spawners[waveIndex] = child.GetComponentsInChildren<EnemySpawner>();
                waveIndex++;
            }

            StartCoroutine(CheckForCameraPosition());

#if UNITY_EDITOR
            Selection.selectionChanged = () =>
            {
                if (Application.isPlaying)
                {
                    return;
                }

                if (Selection.activeTransform == null)
                {
                    return;
                }

                if (Selection.activeTransform.GetComponent<EnemyWaveController>() != null)
                {
                    cam.transform.position = Selection.activeTransform.position;
                    CameraUtils.DrawCameraEdgesAt(Selection.activeTransform.position);
                }
            };
#endif
        }

        private IEnumerator CheckForCameraPosition()
        {
            while (CameraUtils.GetRightEdgeWorldPosition() < transform.position.x)
            {
                yield return new WaitForSeconds(1f);
            }
            Camera.main.transform.DOMoveX(transform.position.x, 2f)
                .OnComplete( StartWaves );
        }

        private void StartWaves()
        {
            currentWaveIndex = 0;
            currentWaveGameObject = waves[currentWaveIndex].gameObject;


            if (cam != null)
            {
                cam.followActive = false;
            }
            if (masterEnemySpawner != null)
            {
                masterEnemyHealth = masterEnemySpawner.GetEnemy().GetComponent<HealthManager>();
            }

            enabled = true;
            StartCoroutine(CurrentWaveUpdate());
        }

        private IEnumerator CurrentWaveUpdate()
        {
            while (enabled)
            {
                StartCurrentWave();

                while (!IsCurrentWaveOver() && IsMasterEnemyAlive())
                {
                    yield return new WaitForSeconds(updatePeriod);
                }

                if (currentWaveIndex == waves.Count - 1  || !IsMasterEnemyAlive())
                {
                    AllWavesOver();
                }
                else
                {
                    currentWaveIndex++;
                }
            }
        }

        // just spwan all the enemies from a wave
        private void StartCurrentWave()
        {
            foreach (EnemySpawner spawner in spawners[currentWaveIndex])
            {
                spawner.Spawn();
            }
        }

        private bool IsCurrentWaveOver()
        {
            bool waveOver = true;
            for (int i = 0; i < spawners[currentWaveIndex].Length; i++)
            {
                if (spawners[currentWaveIndex][i].IsEnemyAlive())
                {
                    waveOver = false;
                }
            }
            return waveOver;
        }

        private void AllWavesOver()
        {
            cam.followActive = true;
            enabled = false;
            EventManager.TriggerEvent(GlobalEvents.WaveEventEnd);
            StopAllCoroutines();
        }

        private bool IsMasterEnemyAlive()
        {
            if (masterEnemyHealth == null)
            {
                return true;
            }
            else
            {
                return masterEnemyHealth.currentHP > 1;
            }
        }
    }
}

using UnityEngine;
using SlugLib;
using System.Collections;

public class TowerBoss : MonoBehaviour {

    public Tower TL;
    public Tower TC;
    public Tower TR;
    private Tower[] towers;
    private int towersDestroyed;
    private TimeUtils timeUtils;
    //TODO start boss another way
    private BoxCollider2D startBossCollider;
    public FollowTarget cam;
    public float timeBetweenMissiles = 2f;
    private Coroutine sequencePhase1;
    public GameObject enemySpwanerPhase2;

    private Tower[][][] masterCombi;

    void Awake() {
        timeUtils = GetComponent<TimeUtils>();
        startBossCollider = GetComponent<BoxCollider2D>();
    }

	void Start () {
        var combi = new[]
        {
            new [] { TL },
            new [] { TC },
            new [] { TR }
        };
        var combi2 = new[]
        {
            new [] { TR, TC, TL },
        };
        var combi3 = new[]
        {
            new [] { TR },
            new [] { TC },
            new [] { TL },
        };
        var combi4 = new[]
        {
            new [] { TR, TL },
            new [] { TC },
            new [] { TL, TR },
        };

        masterCombi = new[] { combi, combi2, combi3 };
        towers = new[] { TL, TR, TC};
    }

	private IEnumerator ProcessSequence() {
        while (true) {
            int randCombiIndex = UnityEngine.Random.Range(0, masterCombi.Length);
            for (int i = 0; i < masterCombi[randCombiIndex].Length; i++) {
                for (int ii = 0; ii < masterCombi[randCombiIndex][i].Length; ii++) {
                    masterCombi[randCombiIndex][i][ii].Fire(0.5f);
                }
                yield return new WaitForSeconds(timeBetweenMissiles);
            }
        }
    }

    private IEnumerator CheckTowerHealth() {
        bool bossAlive = true;
        while (bossAlive) {
            int currentTowersDestroyed = 0;
            for(int i=0; i<towers.Length; i++) {
                if (towers[i].Dead) {
                    currentTowersDestroyed++;
                }
            }
            
            if (currentTowersDestroyed != towersDestroyed) {
                timeBetweenMissiles -= 0.4f;
                if (currentTowersDestroyed == 1) {
                    FirstTowerDestroyed();
                } else if (currentTowersDestroyed == 2) {

                }
                towersDestroyed = currentTowersDestroyed;
            }

            if (currentTowersDestroyed == towers.Length) {
                EventManager.Instance.TriggerEvent(GlobalEvents.BossDead);
                bossAlive = false;
            }

            yield return new WaitForSeconds(1f);
        }
    }

    void FirstTowerDestroyed() {
        enemySpwanerPhase2.SetActive(true);
        if (sequencePhase1 != null) {
            StopCoroutine(sequencePhase1);
        }
        for(int i=0; i<towers.Length; i++) {
            if (!towers[i].Dead) {
                towers[i].EnableAgressiveMode();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (col.tag == "Player") {
            OpenTowers();
            startBossCollider.enabled = false;
            cam.followActive = false;
        }
    }

    void OpenTowers() {
        EventManager.Instance.TriggerEvent(GlobalEvents.BossStart);
        TR.OpenTower(()=> {
            sequencePhase1 = StartCoroutine("ProcessSequence");
        });
        TL.OpenTower(() => {});
        TC.OpenTower(() => {});

        StartCoroutine("CheckTowerHealth");
    }

}

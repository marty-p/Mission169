using UnityEngine;
using Mission169;
using System;

public class SoldierEvents : EnemyEvents {

    private SoldierAnimationManager animManager;
    //Need a base class BRAIN, not sure anymore since brain does not have StartTask method anymore
    private EnemyBrain brain;
    private TimeUtils timeUtils;
    private Blink blink;

    void Awake() {
        animManager = GetComponent<SoldierAnimationManager>();
        timeUtils = GetComponent<TimeUtils>();
        blink = GetComponent<Blink>();
        brain = GetComponent<EnemyBrain>();
    }

    public override void OnInit() {
        if (brain != null) {
            brain.Reset();
        }
    }

    public override void OnDead(ProjectileProperties proj) {
        animManager.PlayDeathAnimation(proj);
        brain.Pause();
    }

    public void AEEndOfDeathAnim() {
        timeUtils.TimeDelay(0.25f, () => {
            blink.BlinkPlease(() => {
                gameObject.SetActive(false);
            });
        });
    }

    public override void OnPlayerDies() {
        animManager.Laugh();
        brain.Pause();
    }

    public override void OnPlayerSpawned() {
        animManager.Reset();
        animManager.ChanceToBeScared(1);
        brain.Reset();
    }

    public override void OnMissionSuccess() {
        animManager.Laugh();
    }

    public override void OnHit(ProjectileProperties proj) {}
}

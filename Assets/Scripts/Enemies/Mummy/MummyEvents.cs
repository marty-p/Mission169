using UnityEngine;
using System.Collections;
using Mission169;
using System;

public class MummyEvents : EnemyEvents {

    private FlashUsingMaterial flashRed;
    private Animator animator;
    private SlugAudioManager audioManager;
    private EnemyBrain brain;

    void Start() {
        flashRed = GetComponent<FlashUsingMaterial>();
        animator = GetComponent<Animator>();
        audioManager = GetComponent<SlugAudioManager>();
        brain = GetComponent<MummyBrain>();
    }

    public override void OnDead(ProjectileProperties proj) {
        animator.SetTrigger("death");
        audioManager.PlaySound(0);
        brain.Pause();
    }

    public override void OnHit(ProjectileProperties proj) {
        flashRed.FlashSlugStyle();
    }

    public override void OnInit() {
        if (brain != null) {
            brain.Reset();
        }
    }

    public override void OnMissionSuccess() {
    }

    public override void OnPlayerDies() {
    }

    public override void OnPlayerSpawned() {
    }
}

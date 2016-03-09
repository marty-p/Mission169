using UnityEngine;
using System;
using AnimatorStateMachineUtil;

public class AnimationManager : MonoBehaviour, IObserver {

    public Animator topAnimator;
    public Animator bottomAnimator;

    void LateUpdate() {
    }

    public void Observe(SlugEvents ev) {
        if (ev == SlugEvents.JumpLowSpeed || ev == SlugEvents.Fall) {
            topAnimator.SetBool("jump_fall", true);
            bottomAnimator.SetBool("jump_fall", true);
        } else if (ev == SlugEvents.JumpHighSpeed) {
            topAnimator.SetBool("jump_high_speed", true);
            bottomAnimator.SetBool("jump_high_speed", true);
        } else if (ev == SlugEvents.HitGround) {
            topAnimator.SetBool("jump_fall", false);
            bottomAnimator.SetBool("jump_fall", false);
            topAnimator.SetBool("jump_high_speed", false);
            bottomAnimator.SetBool("jump_high_speed", false);
        } else if (ev == SlugEvents.Turn) {
            topAnimator.SetTrigger("turn");
        } else if (ev == SlugEvents.StartMoving) {
            topAnimator.SetBool("walking", true);
            bottomAnimator.SetBool("walking", true);
        } else if (ev == SlugEvents.StopMoving) {
            topAnimator.SetBool("walking", false);
            bottomAnimator.SetBool("walking", false);
        } else if (ev == SlugEvents.Sit) {
            if (!topAnimator.GetBool("sat")) {
                topAnimator.SetTrigger("sit");
            }
            topAnimator.SetBool("sat", true);
        } else if (ev == SlugEvents.Stand) {
            topAnimator.SetBool("sat", false);
            topAnimator.SetBool("look_up", false);
        } else if (ev == SlugEvents.LookUp) {
            if (!topAnimator.GetBool("look_up") ) {
                topAnimator.SetBool("look_up", true);
            }
        }
    }





}

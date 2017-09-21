using System;
using UnityEngine;

public class Berserker : MonoBehaviour, IReceiveDamage {

    public GameObject topBody;
    public GameObject bottomBody;
    public GameObject bloodSplash;
    public GameObject limbs;
    private Animator bottomBodyAnimator;
    private SlugPhysics physic;
    private AreaOfEffectProjectile knife;
    private Collider2D col;
    private TimeUtils timeUtils;
    public bool walkingMode = true;

    void Awake() {
        physic = GetComponent<SlugPhysics>();
        knife = GetComponent<AreaOfEffectProjectile>();
        bottomBodyAnimator = bottomBody.GetComponent<Animator>();
        col = GetComponent<Collider2D>();
        timeUtils = GetComponent<TimeUtils>();
    }

    void OnEnable() {
        bottomBody.SetActive(true);
        bloodSplash.SetActive(false);
        RepositionLimbs();
        limbs.SetActive(false);
        transform.localPosition = Vector3.zero;
        if (!walkingMode) {
            bottomBodyAnimator.Play("berserker-sitting");
        }
    }

    private void OnBecameInvisible()
    {
        gameObject.SetActive(false);
    }

    public void Update() {
        if (walkingMode) {
            physic.MoveForward(1.3f);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player") {
            Attack();
        }
    }

    private void Attack() {
        topBody.SetActive(true);
        bottomBodyAnimator.SetBool("attack", true);
        knife.CastAOE("Player", transform.position);
    }

    public void OnDamageReceived(ProjectileProperties projectileProp, int newHP) {
        if (!enabled) {
            return;
        }
        if (newHP < 1) {
            gameObject.SetActive(false);
            bloodSplash.SetActive(true);
            bloodSplash.transform.position = topBody.transform.position;
            limbs.transform.position = topBody.transform.position;
            limbs.SetActiveRecursively(true);
        }
    }

    public void SetWalkingMode(bool walking) {
        walkingMode = walking;
        if (bottomBodyAnimator.isInitialized) {
            if (walking)
            {
                bottomBodyAnimator.SetTrigger("walk");
            }
            else
            {
                bottomBodyAnimator.Play("berserker-sitting");
            }
        }
    }

    private void RepositionLimbs() {
        foreach (Transform bodyPart in limbs.transform) {
            bodyPart.transform.position = limbs.transform.position;
        }
    }

}

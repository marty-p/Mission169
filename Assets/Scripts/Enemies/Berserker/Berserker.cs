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
        limbs.SetActive(false);
        RepositionLimbs();
        col.enabled = true;
        physic.enabled = true;
        if (!walkingMode) {
            bottomBodyAnimator.Play("berserker-sitting");
        }
    }

    public void FixedUpdate() {
        if (walkingMode) {
            physic.MoveForward(35);
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
        if (newHP < 1) {
            DesInit();
            bloodSplash.SetActive(true);
            limbs.SetActive(true);
        }
    }

    public void SetWalkingMode() {
        walkingMode = true;
        bottomBodyAnimator.SetTrigger("walk");
    }

    private void DesInit() {
        bottomBody.SetActive(false);
        topBody.SetActive(false);
        physic.enabled = false;
        col.enabled = false;
        timeUtils.TimeDelay(2, () => { gameObject.SetActive(false); });
    }

    private void RepositionLimbs() {
        foreach (Transform bodyPart in limbs.transform) {
            bodyPart.transform.position = limbs.transform.position;
        }
    }

}

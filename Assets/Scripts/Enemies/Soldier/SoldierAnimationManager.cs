using System;
using UnityEngine;
using Utils;

public class SoldierAnimationManager : MonoBehaviour, IObserver {

    private Animator anim;
    public Animator blood;
    private string[] deathTriggers;
    private BoxCollider2D boxCollider;
    private TimeUtils timeUtils;
    private float minHeightToFall = -0.2f;
    private float pastXpos;
    private Vector3 pastDir;
    public bool IsWalkingBackward { get; set; }

    void Awake() {
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        timeUtils = GetComponent<TimeUtils>();
        pastDir = transform.right;
        pastXpos = transform.position.x;
    }

    void Start () {
        deathTriggers = new String[3];
        deathTriggers[0] = "hit_by_bullet1";
        //deathTriggers[1] = "hit_by_bullet2";
        deathTriggers[1] = "hit_by_bullet3";
        deathTriggers[2] = "hit_by_bullet4";
	}

    public void Observe(SlugEvents ev) {
        if (ev == SlugEvents.HitGround) {
            if (anim.GetBool("hit_by_grenade")) {
                anim.SetBool("hit_by_grenade", false);
            } else if(anim.GetBool("fall_death")) {
                anim.SetBool("fall_death", false);
            } else {
                anim.SetTrigger("hit_ground");
            }
        } else if(ev == SlugEvents.Fall) {
            anim.SetTrigger("falls");
        }
    }

    public void PlayDeathAnimation(ProjectileProperties proj) {
        ProjectileType bulletType = proj.type;
        if (bulletType == ProjectileType.Bullet) {
            //TODO raycast to see if there is other world collider beneath
            if (transform.position.y > minHeightToFall) {
                anim.SetBool("fall_death", true);
            } else {
                string deathTrigger = GetRandomDeathTrigger();
                anim.SetTrigger(deathTrigger);
            }

            blood.Play("1");
        } else if (bulletType == ProjectileType.Grenade) {
            anim.SetBool("hit_by_grenade", true);
        } else if (bulletType == ProjectileType.Knife) {
            anim.SetTrigger("slashed");
        }
    }

    public void AEStartDeathFall() {
        boxCollider.enabled = false;
        timeUtils.TimeDelay( 0.2f,() => { boxCollider.enabled = true;});
    }

    private string GetRandomDeathTrigger() {
        float randFloat = UnityEngine.Random.value * deathTriggers.Length;
        int randInt = (int)randFloat;
        return deathTriggers[randInt];
    }

    public void Laugh() {
        anim.SetTrigger("laugh");
    }

    public void WalkBackward() {
        anim.SetTrigger("walk_backward");
        IsWalkingBackward = true;
    }
    public void AEWalkbackwardDone() {
        IsWalkingBackward = false;
    }

    public void ChanceToBeScared(float chance) {
       if (UnityEngine.Random.value < chance) {
            anim.SetTrigger("scared");
        }
       //TODO have AnimEvent at the end of scared that does:
//                   if (UnityEngine.Random.value < getScaredFactor) {
//                        anim.SetTrigger("run_away");
//                    }
    }

    public void Reset() {
        anim.Rebind();
    }

    void Update() {
        float currentPos = (float) Math.Truncate(transform.position.x * 1000) / 1000;
        float dx = currentPos - pastXpos;
        bool isInMotion = Mathf.Abs(dx) > 0;
        bool goForward = (dx < 0 && transform.right == Vector3.left) || (dx > 0 && transform.right == Vector3.right);

        if (isInMotion) {
            if (goForward) {
                anim.SetBool("walking", true);
                anim.SetBool("walking_backward", false);
            } else {
                anim.SetBool("walking_backward", true);
            }
        } else {
            anim.SetBool("walking", false);
            anim.SetBool("walking_backward", false);
        }
        // Turn Around Anim when needed
        if (transform.right != pastDir) {
                anim.SetTrigger("turn_around");
        }

        pastXpos = currentPos;
        pastDir = transform.right;
    }
}

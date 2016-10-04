using UnityEngine;

public class MissileLauncher : MonoBehaviour {

    public GameObject smoke;
    private Animator anim;
    public ObjectPoolScript missilePool;
    public Vector3 missileInitialPos;
    private TimeUtils timeUtils;
    private bool shootInProgress;
    private float speedFactor;

    void Awake() {
        anim = GetComponent<Animator>();
        timeUtils = GetComponent<TimeUtils>();
    }

    public void Shoot(float speedFactor = 1) {
        if (!shootInProgress) {
            anim.SetTrigger("shoot");
            shootInProgress = true;
            this.speedFactor = speedFactor;
        }
    }

    public void AEEndShootAnim() {
        smoke.SetActive(false);
        smoke.SetActive(true);
        GameObject missile = missilePool.GetPooledObject();
        missile.transform.localPosition = missileInitialPos;
        missile.SetActive(true);
        GuidedMissile guidedMissile = missile.GetComponentInChildren<GuidedMissile>();
        guidedMissile.speedFactor = speedFactor;
        missile.GetComponentInChildren<GuidedMissile>().speedFactor = speedFactor;
        shootInProgress = false;
    }

}

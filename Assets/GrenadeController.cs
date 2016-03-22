using UnityEngine;

public class GrenadeController : MonoBehaviour, IObserver {

    private PhysicsSlugEngine physics;
    private Vector3 ZRotationIncrement;

    private int bounceCount;
    private const int bouncesToExplode = 2;
    public RuntimeAnimatorController grenadeExplosionAnimator;

    // Works for something that is symetric of course
    private Vector3 rightOrientation = new Vector3(0, 0, 0);
    private Vector3 leftOrientation = new Vector3(0, 0, 180);

    void Awake () {
        physics = GetComponent<PhysicsSlugEngine>();
        ZRotationIncrement = new Vector3(0, 0, 250*Time.fixedDeltaTime);
	}

    void OnTriggerEnter2D(Collider2D col) {
        if (col.tag == "enemy") {
            IHitByProjectile[] components = col.GetComponentsInChildren<IHitByProjectile>();
            HealthManager enemyHealthManager = col.GetComponent<HealthManager>();
            enemyHealthManager.OnHitByGrenade();
            Explode();
        }
    }

    public void Init() {
        bounceCount = 0;
        physics.Reset();
    }

    public void Throw(Vector3 dir) {
        // FIXME SetOrientation should be in Init
        SetOrientation(dir);
        physics.SetVelocity(1.5f); 
    }

    public void Explode() {
        Animator anim = SimpleAnimatorPool.GetPooledAnimator();
        anim.runtimeAnimatorController = grenadeExplosionAnimator;
        anim.transform.position = transform.position;
        gameObject.SetActive(false);
    }

    private void SetOrientation(Vector3 dir) {
        if (dir == Vector3.right) {
            transform.eulerAngles = rightOrientation;
        } else if (dir == Vector3.left) {
            transform.eulerAngles = leftOrientation;
        }
    }

    void FixedUpdate () {
        transform.Rotate(ZRotationIncrement);
	}

    public void Observe(SlugEvents ev) {
        if (ev == SlugEvents.HitGround) {
            bounceCount++;
            //ZRotationIncrement *= 0.6f;
            if (bounceCount >= bouncesToExplode) {
                Explode();
            }
        }
    }
}

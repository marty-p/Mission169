using UnityEngine;

public class GrenadeController : MonoBehaviour, IObserver {

    private PhysicsSlugEngine physics;
    private Vector3 ZRotationStepCurrent;
    private Vector3 ZRotationStepInitial;
    private ProjectileController projectileController;

    private int bounceCount;
    public int bouncesToExplode = 2;

    void Awake () {
        physics = GetComponent<PhysicsSlugEngine>();
        ZRotationStepInitial = new Vector3(0, 0, 250*Time.fixedDeltaTime);
        projectileController = GetComponent<ProjectileController>();
	}

    public void Init() {
        ZRotationStepCurrent = ZRotationStepInitial;
        bounceCount = 0;
        physics.Reset();
    }

    public void Throw(Vector3 dir) {
        // FIXME SetOrientation should be in Init
        projectileController.SetOrientation(dir);
        physics.SetVelocity(1.5f); 
    }

    private void Explode() {
        projectileController.Impact();
        gameObject.SetActive(false);
    }

    void FixedUpdate () {
        transform.Rotate(ZRotationStepCurrent);
	}

    public void Observe(SlugEvents ev) {
        if (ev == SlugEvents.HitGround) {
            bounceCount++;
            ZRotationStepCurrent *= 0.5f;
            if (bounceCount >= bouncesToExplode) {
                Explode();
            }
        }
    }
}

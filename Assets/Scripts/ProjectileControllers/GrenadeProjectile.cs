using UnityEngine;

public class GrenadeProjectile : MonoBehaviour, IProjectile {

    public ProjectileProperties properties;
    private PhysicsSlugEngine physics;
    private Vector3 ZRotationStepCurrent;
    private Vector3 ZRotationStepInitial;
    private int bounceCount;
    public int bouncesToExplode = 2;
    private bool launched;
    private float x;
    private float xOffset;
    private float yOffset;
    private float dist;
    private float xIncrement;
    private float dir;
    private bool onTheLinearPart;
    private float x_inc2;
    private float y_inc2;

    void Awake () {
        physics = GetComponent<PhysicsSlugEngine>();
        ZRotationStepInitial = new Vector3(0, 0, -820*Time.fixedDeltaTime);
	}

    public void OnTriggerEnter2D(Collider2D col) {
        if (col.tag == properties.victimTag) {
            Explode(col);
        } else if (col.tag == "World") {
            bounceCount++;
            ZRotationStepCurrent *= 2;
//            if (bounceCount >= bouncesToExplode) {
                Explode(col);
//            }
        }
    }
/*
    public void Observe(SlugEvents ev) {
        if (ev == SlugEvents.HitGround) {
            bounceCount++;
            ZRotationStepCurrent *= 2;
            if (bounceCount >= bouncesToExplode) {
                Explode();
            }
        }
    }
    */
    private void Explode(Collider2D col) {
        ProjectileUtils.ImpactAnimation(transform, col, properties);
        ProjectileUtils.NotifyCollider(col, properties);
        gameObject.SetActive(false);
        //tweenTest.Kill();
    }

    void FixedUpdate () {
        if (launched) {
            transform.Rotate(ZRotationStepCurrent);
            float a = 0.25f;
            float y = 0;
            if (!onTheLinearPart) {
                float dist2 = 1.6f;
                y = -(dist2 * x) * (dist2 * x);
                x_inc2 -= 0.047f;
                x += x_inc2*Time.fixedDeltaTime;
            } else if (x < 0.4f) {
                x += x_inc2 * Time.fixedDeltaTime;
                x_inc2 -= 0.022f;
                y = a * x;
            } else if ( onTheLinearPart) {
                onTheLinearPart = false;
                y = 0;
                xOffset = transform.position.x;
                yOffset = transform.position.y;
                x = 0;
                float dist2 = 1.2f;
                y = -(dist2 * x) * (dist2 * x);
                x +=  x_inc2*Time.fixedDeltaTime;
            }
            transform.position = new Vector2(xOffset + x * dir, yOffset + y);
        }
	}

    public void Launch(string victimsTag, Vector2 destination) {
        launched = true;
        ZRotationStepCurrent = ZRotationStepInitial;
        properties.victimTag = victimsTag;

        x = 0;
        dir = transform.right.x;
        xOffset = transform.position.x;
        yOffset = transform.position.y;
        onTheLinearPart = true;
        x_inc2 = 2.3f;

        //FIXME not spinning in right sens when throwing to the left
        // Starting angle not correct when spinning to the left
        bounceCount = 0;
/*
        Vector3[] wayPoints = new Vector3[2];
        float x1 = 0.74f * transform.right.x;
        float y1 = -0.06f;

        float x2 = 1f * transform.right.x;
        float y2 = -1.9f;

        wayPoints[0] = new Vector2(transform.position.x + x1, transform.position.y + y1);
        wayPoints[1] = new Vector2(transform.position.x + x2, transform.position.y + y2 );

        transform.eulerAngles = new Vector3(0, 0, 210);
        tweenTest = transform.DOPath(wayPoints, 2.4f, PathType.CatmullRom,
                PathMode.Sidescroller2D, 10).SetEase(Ease.Linear).SetSpeedBased();
                */
    }

    void OnBecameInvisible() {
        gameObject.SetActive(false);
        launched = false;
    }

}

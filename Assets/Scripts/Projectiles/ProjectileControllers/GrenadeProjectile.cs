using UnityEngine;

public class GrenadeProjectile : MonoBehaviour, IProjectile {

    public ProjectileProperties properties;
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
    private bool linearPart;
    private float x_inc2;
    private float y_inc2;

    void Awake () {
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
        ProjectileUtils.ImpactAnimationAndSound(transform, col, properties);
        ProjectileUtils.NotifyCollider(col, properties);
        gameObject.SetActive(false);
    }

    void FixedUpdate () {
        if (launched) {
            transform.Rotate(ZRotationStepCurrent);
            float a = 0.25f;
            float y = 0;
            if (!linearPart) {
                float dist2 = 1.6f;
                y = -(dist2 * x) * (dist2 * x);
                x_inc2 -= 0.047f;
                x += x_inc2*Time.fixedDeltaTime;
            } else if (x < 0.4f) {
                x += x_inc2 * Time.fixedDeltaTime;
                x_inc2 -= 0.022f;
                y = a * x;
            } else if (linearPart) {
                linearPart = false;
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
        linearPart = true;
        x_inc2 = 2.3f;

        //FIXME not spinning in right sens when throwing to the left
        // Starting angle not correct when spinning to the left
        bounceCount = 0;
    }

    void OnBecameInvisible() {
        gameObject.SetActive(false);
        launched = false;
    }

}

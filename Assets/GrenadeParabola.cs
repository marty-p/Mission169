using UnityEngine;

public class GrenadeParabola : MonoBehaviour, IProjectile {

    public ProjectileProperties properties;

    private Vector3 ZRotationStep;
    private bool launched;

    private float x;
    private float xOffset;
    private float yOffset;
    private float dist;
    private float xIncrement;
    private float dir;

    void Awake () {
        ZRotationStep = new Vector3(0, 0, -820*Time.fixedDeltaTime);
	}

    void FixedUpdate () {
        if (launched) {
            transform.Rotate(ZRotationStep);

            float height = 0.8f;
            // +height^2 is added to make sure that f(0) is 0 and not height^2
            float y = -(dist*x - height) * (dist*x - height) + height*height;
            transform.position = new Vector2(xOffset + x * dir, yOffset + y);
            x += xIncrement;
        }
	}

    public void Launch(string victimsTag, Vector2 destination) {
        launched = true;
        properties.victimTag = victimsTag;

        x = 0;
        xOffset = transform.position.x;
        yOffset = transform.position.y;
        dir = transform.right.x;

        float distWithTarget = Mathf.Abs(destination.x - transform.position.x);
        xIncrement = (distWithTarget/1.3f)*Time.fixedDeltaTime;
        
        // 1.5f est trouve de maniere empirique. Plus dir est grand plus la parabole est petite
        // just a way to get the grenade to land aproximately where the player is
        dist = 1.5f *( 1 / distWithTarget);
    }

    public void OnTriggerEnter2D(Collider2D col) {
        if (col.tag == properties.victimTag) {
            Explode(col);
        } else if (col.tag == "World") {
            Explode(col);
        }
    }

    private void Explode(Collider2D col) {
        ProjectileUtils.ImpactAnimation(transform, col, properties);
        ProjectileUtils.NotifyCollider(col, properties);
        gameObject.SetActive(false);
    }

    void OnBecameInvisible() {
        gameObject.SetActive(false);
        launched = false;
    }
}

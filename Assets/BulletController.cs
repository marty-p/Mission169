using UnityEngine;
using System.Collections;
using System;

[Serializable]

public class BulletProperties
{
    public int strength = 1;
    public int speedInpixelsPerSecond = 5;
}

public class BulletController : MonoBehaviour {

    public BulletProperties bulletProperties;
    public SpriteRenderer spriteRenderer;
    public AnimationClip bulletImpactClip;
    private float onScreenFor = 0;

    // Works for something that is fully symetric of course
    private Vector3 rightOrientation = new Vector3(0, 0, 0);
    private Vector3 leftOrientation = new Vector3(0, 0, 180);
    private Vector3 upOrientation = new Vector3(0, 0, 90);
    private Vector3 downOrientation = new Vector3(0, 0, -90);
   
    void OnTriggerEnter2D(Collider2D col) {
        if (col.tag == "enemy") {
            Impact();
            IHitByProjectile component = col.GetComponentInChildren<IHitByProjectile>();
            component.OnHitByProjectile(bulletProperties.strength, (int)transform.right.x);
            gameObject.SetActive(false);
        }
    }

    void Update() {
        onScreenFor += Time.deltaTime;
        if (onScreenFor > 0.08f) {
            spriteRenderer.enabled = true;
        }
    }

    void Impact() {
  //      SimpleAnimationController animController =
   //             SimpleAnimPool.GetPooledSimpleAnimController();
   //     animController.SetAnimationClip(bulletImpactClip);
   //     animController.transform.position = transform.position;
    }

    void FixedUpdate() {
        UpdatePosition(Time.fixedDeltaTime);
    }

    void OnBecameInvisible() {
        gameObject.SetActive(false);
    }

    public void InitPosition(Vector2 pos)
    {
        transform.position = pos;
    }

    public void Show(bool shown)
    {
        spriteRenderer.enabled = shown;
    }

    public void Fire(Vector3 direction)
    {
        SetOrientation(direction);
        
        spriteRenderer.enabled = false;
        onScreenFor = 0;
    }

    private void SetOrientation(Vector3 dir) {
        if (dir == Vector3.right) {
            transform.eulerAngles = rightOrientation;
        } else if (dir == Vector3.left) {
            transform.eulerAngles = leftOrientation;
        } else if (dir == Vector3.up) {
            transform.eulerAngles = upOrientation;
        } else if (dir == Vector3.down) {
            transform.eulerAngles = downOrientation;
        }
    }

    void UpdatePosition(float dt) {
        transform.Translate( Vector3.right * 5 * dt);
    }

}

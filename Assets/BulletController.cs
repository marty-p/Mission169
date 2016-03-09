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
    public Rigidbody2D rigidBody;
    public BoxCollider2D boxCollider;
    public SpriteRenderer spriteRenderer;
    public float onScreenFor = 0;
    private int vel;

    // Works for something that is symetric of course
    private Vector3 rightOrientation = new Vector3(0, 0, 0);
    private Vector3 leftOrientation = new Vector3(0, 0, 180);
    private Vector3 upOrientation = new Vector3(0, 0, 90);
    private Vector3 downOrientation = new Vector3(0, 0, -90);
   
    void OnTriggerEnter2D(Collider2D col) {
        if (col.tag == "enemy") {
            Desactivate();
            IHitByProjectile[] components = col.GetComponentsInChildren<IHitByProjectile>();
            foreach (IHitByProjectile component in components) {
                component.OnHitByProjectile(bulletProperties.strength, (int)transform.right.x);
            }
        }
    }

    void Update() {
        onScreenFor += Time.deltaTime;
        if (onScreenFor > 0.08f) {
            spriteRenderer.enabled = true;
        }
    }

    void FixedUpdate() {
        UpdatePosition(Time.fixedDeltaTime);
    }

    void OnBecameInvisible()
    {
        Desactivate();
    }

    private void Desactivate()
    {
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

    public void UpdatePosition(float dt) {
        transform.Translate( Vector3.right * 5 * dt);
    }

    public void StopMovement()
    {
    }

    public void EnableCollision(bool enabled)
    {
        boxCollider.enabled = enabled;
    }

}

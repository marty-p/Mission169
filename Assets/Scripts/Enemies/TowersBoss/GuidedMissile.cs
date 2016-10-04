using UnityEngine;
using System.Collections;
using System;

public class GuidedMissile : MonoBehaviour, IReceiveDamage {

    private Transform player;
    public float speedFactor = 1.5f;
    private float initialZangle;
    public ProjectileProperties properties;
    public Animator explosion;
    private FlashUsingMaterial flash;

    void Awake() {
        flash = GetComponent<FlashUsingMaterial>();
    }

	void Start () {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        initialZangle = transform.eulerAngles.z;
	}
	
	void Update () {
        UpdateZAngle();	
        UpdatePosition();
	}

    void OnEnable() {
        transform.localPosition = Vector3.zero;
        transform.eulerAngles = new Vector3(0, 0, -180);
    }

    public void SetSpeedFactor(float speedFactor) {
        this.speedFactor = speedFactor;
    }

    public void OnTriggerEnter2D(Collider2D col) {
        if (col.tag == "Player") {
            Explode(col);
        } else if (col.tag == "World") {
            Explode(col);
        }
    }

    private void Explode(Collider2D col = null) {
        explosion.transform.position = this.transform.position;
        explosion.gameObject.SetActive(true);
        if (col != null) {
            ProjectileUtils.NotifyCollider(col, properties);
        }

        flash.ResetMaterial();
        gameObject.SetActive(false);
    }

    void UpdatePosition() {
        float transX = transform.up.x * Time.deltaTime * speedFactor;
        float transY = transform.up.y * Time.deltaTime * speedFactor;
        transform.Translate(transX, transY, 0, Space.World);
    }

    void UpdateZAngle() {
        float dx = transform.position.x - player.position.x;
        float dxAbs = Mathf.Abs(dx);
        float dy = Mathf.Abs(transform.position.y - player.position.y - 0.1f);

        float angleZ;
        angleZ = dy == 0 ? 0 : Mathf.Atan(dxAbs/dy);
        angleZ = Mathf.Rad2Deg * angleZ * Math.Sign(dx);
        angleZ = Mathf.MoveTowardsAngle(transform.eulerAngles.z, initialZangle - angleZ, Time.deltaTime*speedFactor*35);

        transform.eulerAngles = new Vector3(0, 0, angleZ);
    }

    public void OnDamageReceived(ProjectileProperties projectileProp, int newHP) {
        if (newHP < 1) {
            Explode();
        } else {
            flash.FlashSlugStyle();
        }
    }
}

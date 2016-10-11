using UnityEngine;

public class DebrisManager : MonoBehaviour {

    private SlugPhysics physic;
    private float velX;
    private float velY;
    public bool spin;
    private float ZRotationFactorInitial = 10;
    private Vector3 ZRotationStep;
    private Blink blink;
    public bool blinkAtTheEnd;
    public bool fallOnly;
    private bool blinking;
    private SpriteRenderer spriteRenderer;

    void Awake() {
        physic = GetComponent<SlugPhysics>();
        blink = GetComponent<Blink>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        velX = UnityEngine.Random.Range(-2.5f, 1.5f);
        velY = UnityEngine.Random.Range(0.5f, 3f);
        if (fallOnly) {
            velY /= 10;
        }
    }

    void OnEnable() {
        blinking = false;
        physic.enabled = true;
        physic.SetVelocityX(velX);
        physic.SetVelocityY(velY);
        ZRotationStep.z =  ZRotationFactorInitial;
        spriteRenderer.enabled = true;
    }

    void LateUpdate() {
        if (Mathf.Approximately(physic.GetVelocity().y, 0)) {
            Desactivate();
        } else if (spin && ZRotationStep.z > 0) {
            transform.Rotate(ZRotationStep);
            ZRotationStep.z = Mathf.MoveTowards(ZRotationStep.z, 0, ZRotationFactorInitial * Time.deltaTime/1.5f);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "World") {
            Desactivate();
        }
    }

    private void Desactivate() {
        physic.enabled = false;
        if (blinkAtTheEnd && !blinking) {
            blinking = true;
            blink.BlinkPlease(() => { gameObject.SetActive(false); });
        } else if (!blinkAtTheEnd) {
            gameObject.SetActive(false);
        }
    }
}

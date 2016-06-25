using UnityEngine;

public class PhysicsSlugEngine : MonoBehaviour {

    private BoxCollider2D boxCollider;
    private IObserver[] observers;

    public float groundDrag = 0;
    public float airDrag = 0.998f;

    public float initialJumpVelocity = 3f;
    public float maxVerticalVelocity = -3;
    public float verticalDrag = 7f;
    public float bounceFactor = 0;

    public float groundMovementFactor = 1.1f;
    public float airLowVelocityMovementFactor = 0.95f;
    public float airHighVelocityMovementFactor = 1.4f;
    private const float rayCastRestLength = 0.03f;
    private float movementFactor = 1.1f; // TODO movementFactor at init is the same as groundMovementFactor

    private Vector2 absoluteVelocity;

    private RaycastHit2D underMyFeetHit;
    private RaycastHit2D inFrontOfMe;

    private bool inTheAir = false;
    public bool InTheAir { get { return inTheAir; } }
    private Vector2 groundSlope;
    private Vector2 rayCastStartPoint;
    private float forceX;

    void Awake() {
        boxCollider = GetComponent<BoxCollider2D>();
        rayCastStartPoint = new Vector2();
        observers = GetComponents<IObserver>();
        absoluteVelocity = new Vector2();
    }


    void FixedUpdate() {
        WhatIsUnderMyFeet();
        UpdateGroundSlope();

        UpdateFallingSpeed();

        float transX = calculateXTranslation();
        float transY = calculateYTranslation();
        transform.Translate(transX, transY, 0, Space.World);

        CalculateVelocity();
        /*
                Debug.DrawLine(rayCastStartPoint, 
                        new Vector2(boxCollider.bounds.min.x + boxCollider.bounds.size.x/2,
                        boxCollider.bounds.min.y + absoluteVelocity.y * Time.fixedDeltaTime - rayCastRestLength));
        */

    }

    float calculateXTranslation() {
        float xTranslation = groundSlope.x * absoluteVelocity.x * movementFactor * Time.fixedDeltaTime;

        WhatIsInFrontOfMe(xTranslation);
        if (inFrontOfMe.collider != null) {
            xTranslation = inFrontOfMe.distance;
        }

        return xTranslation;
    }

    float calculateYTranslation() {
        float yTranslation = 0;
        bool justFellFromPlatform = underMyFeetHit.collider == null && !inTheAir;
        bool justLanding = underMyFeetHit.collider != null && inTheAir && absoluteVelocity.y < 0;
        bool justBouncing = justLanding && bounceFactor > 0;
        if (justFellFromPlatform) {
            StartFalling();
        } else if (justBouncing) {
            yTranslation = 0;
            absoluteVelocity.y = initialJumpVelocity * bounceFactor;
            //            absoluteVelocity.x = bounceFactor*5;
            NotifyObservers(SlugEvents.HitGround);
        } else if (justLanding) {
            StopFalling();
            yTranslation = FixYPosition(underMyFeetHit) - transform.position.y;
        } else if (inTheAir) {
            yTranslation = absoluteVelocity.y * Time.fixedDeltaTime;
        } else {
            // I may be going up/down a hill
            yTranslation = FixYPosition(underMyFeetHit) - transform.position.y;
        }
        return yTranslation;
    }

    void WhatIsUnderMyFeet() {
        rayCastStartPoint = new Vector2(boxCollider.bounds.min.x + boxCollider.bounds.size.x / 2,
                boxCollider.bounds.min.y - 0.00005f);
        Vector2 dir;
        if (absoluteVelocity.y > 0) {
            dir = Vector2.up;
        } else {
            dir = Vector2.down;
        }

        int layerMask = 1 << LayerMask.NameToLayer("World");
        underMyFeetHit = Physics2D.Raycast(rayCastStartPoint, dir,
                rayCastRestLength + Mathf.Abs(absoluteVelocity.y * Time.fixedDeltaTime),
                layerMask);
    }

    void WhatIsInFrontOfMe(float translationPlanned) {
        float startX;
        if ((Vector2)transform.right == Vector2.right) {
            startX = boxCollider.bounds.max.x + 0.00005f;
        } else {
            startX = boxCollider.bounds.min.x - 0.00005f;
        }
        
        Vector2 startPoint = new Vector2(startX, boxCollider.bounds.max.y);
        Vector2 endPoint = startPoint + new Vector2 (translationPlanned, 0);

        int layerMask = 1 << LayerMask.NameToLayer("EnemySolid") | 1 << LayerMask.NameToLayer("World");
        inFrontOfMe = Physics2D.Linecast(startPoint, endPoint, layerMask);
        Debug.DrawLine(startPoint, endPoint, Color.red);
    }


    float FixYPosition(RaycastHit2D hit) {
        return hit.point.y + boxCollider.bounds.size.y / 2 - boxCollider.offset.y + 0.01f;
    }

    void UpdateGroundSlope() {
        if (inTheAir) {
            groundSlope.x = 1;
            groundSlope.y = 0;
            return;
        }

        Quaternion rotate = Quaternion.Euler(0, 0, -90 * transform.right.x);
        groundSlope = rotate * underMyFeetHit.normal;
        groundSlope.x = Mathf.Abs(groundSlope.x);

        groundSlope.Normalize();
        /*
                Debug.DrawLine(underMyFeetHit.point,
                        new Vector2(underMyFeetHit.point.x + groundSlope.x,  underMyFeetHit.point.y + groundSlope.y),
                        Color.cyan);
        */
    }

    void UpdateFallingSpeed() {
        if (inTheAir) {
            absoluteVelocity.y -= (verticalDrag * Time.fixedDeltaTime);
            Mathf.Clamp(absoluteVelocity.y, maxVerticalVelocity, initialJumpVelocity / 3);
        }
    }

    void StopFalling() {
        inTheAir = false;
        movementFactor = groundMovementFactor;
        absoluteVelocity.y = 0;
        NotifyObservers(SlugEvents.HitGround);
    }

    void StartFalling() {
        inTheAir = true;
        movementFactor = airLowVelocityMovementFactor;
        NotifyObservers(SlugEvents.Fall);
    }

    void CalculateVelocity() {
        if (inTheAir) {
            absoluteVelocity.x *= airDrag;
        } else {
            absoluteVelocity.x =  absoluteVelocity.x * groundDrag + forceX;
        }
    }

    public bool JumpLowVel() {
        if (inTheAir) {
            return false;
        }
        inTheAir = true;

        movementFactor = airLowVelocityMovementFactor;
        absoluteVelocity.y = initialJumpVelocity;
        return true;
    }

    public bool JumpHighVel() {
        if (inTheAir) {
            return false;
        }
        inTheAir = true;

        movementFactor = airHighVelocityMovementFactor;
        absoluteVelocity.y = initialJumpVelocity;
        return true;
    }

    public void SetVelocity(float velX, float velY) {
        absoluteVelocity.x = velX * transform.right.x;
        absoluteVelocity.y = velY * transform.up.y;
    }

    public void SetVelocityX(float velX) {
        absoluteVelocity.x = velX;
    }

    public float GetVelocityX() {
        return absoluteVelocity.x;
    }

    public Vector2 GetVelocity() {
        return absoluteVelocity;
    }

    public void SetForceX(float forceX) {
        this.forceX = forceX;
    }

    public void ChangeDirection(Vector3 newDir) {
        if (transform.right != newDir) {
            transform.right = newDir;
        }
    }

    public void MoveForward(float vel = 20) {
        absoluteVelocity.x = transform.right.x*vel * Time.fixedDeltaTime;
    }

    public void SetMovementFactor(float movementFactor) {
        this.movementFactor = movementFactor;
    }

    public void Reset() {
        inTheAir = false;
        absoluteVelocity = Vector2.zero;
    }

    void NotifyObservers(SlugEvents ev) {
        if (observers == null) {
            return;
        }
        foreach (IObserver obs in observers) {
            obs.Observe(ev);
        }
    }
}

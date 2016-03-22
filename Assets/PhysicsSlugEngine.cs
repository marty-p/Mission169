using UnityEngine;

public class PhysicsSlugEngine : MonoBehaviour {

    private BoxCollider2D boxCollider;
    private IObserver[] observers;

    public float initialJumpVelocity = 3f;
    public float maxVerticalVelocity = -3;
    public float verticalDrag = 7f;
    public float bounceFactor = 0;

    private const float groundMovementFactor = 0.8f;
    private const float airLowVelocityMovementFactor = 1.1f;
    private const float airHighVelocityMovementFactor = 1.22f;
    private const float rayCastRestLength = 0.03f;
    private float movementFactor;
    
    private Vector2 absoluteVelocity;
    private Vector2 previousPos;

    private RaycastHit2D underMyFeetHit;

    private bool inTheAir = false;
    public bool InTheAir { get { return inTheAir;} }
    private float yCandidate;
    private Vector2 groundSlope;
    private Vector2 rayCastStartPoint;

    void Awake () {
        boxCollider = GetComponent<BoxCollider2D>();
        rayCastStartPoint = new Vector2();
        observers = GetComponents<IObserver>();
        yCandidate = transform.position.y;
        absoluteVelocity = new Vector2();
        previousPos = new Vector2(transform.position.x, transform.position.y);
    }

    void FixedUpdate () 
    {
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
                boxCollider.bounds.min.y + currentVerticalVelocity * Time.fixedDeltaTime - rayCastRestLength));
*/
    }

    float calculateXTranslation() {
        return groundSlope.x * absoluteVelocity.x * movementFactor * Time.fixedDeltaTime;
    }

    float calculateYTranslation() {
        float yTranslation = 0;
        bool justFellFromPlatform = underMyFeetHit.collider == null && !inTheAir;
        bool justLanding = underMyFeetHit.collider != null;
        bool justBouncing = justLanding && bounceFactor > 0;
        if (justFellFromPlatform) {
            StartFalling();
        } else if (justBouncing) {
            yTranslation = 0;
            absoluteVelocity.y = initialJumpVelocity * bounceFactor;
            NotifyObservers(SlugEvents.HitGround);
        } else if (justLanding) {
            StopFalling();
            yTranslation = FixYPosition(underMyFeetHit) - transform.position.y;
        } else if (inTheAir) {
            yTranslation = absoluteVelocity.y * Time.fixedDeltaTime;
        } else {
            yTranslation = 0;
        }
        return yTranslation;
    }

    void WhatIsUnderMyFeet() {
        rayCastStartPoint = new Vector2(boxCollider.bounds.min.x + boxCollider.bounds.size.x/2,
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
            absoluteVelocity.y -= (verticalDrag*Time.fixedDeltaTime);
            Mathf.Clamp(absoluteVelocity.y, maxVerticalVelocity, initialJumpVelocity/3);
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
            absoluteVelocity.x *= 0.999f;
        } else {
            absoluteVelocity.x = 0;
        }
    }

    public void Jump() {
        if (inTheAir) {
            return;
        }
        inTheAir = true;

        if (absoluteVelocity.x > 0) {
            movementFactor = airHighVelocityMovementFactor;
            NotifyObservers(SlugEvents.JumpHighSpeed);
        } else {
            movementFactor = airLowVelocityMovementFactor;
            NotifyObservers(SlugEvents.JumpLowSpeed);
        }
        absoluteVelocity.y = initialJumpVelocity;
    }

    public void SetVelocity(float velX) {
        Jump();
        absoluteVelocity.x = velX * transform.right.x;
    }

    public void changeDirection(Vector3 newDir) {
        if (transform.right != newDir) {
            transform.right = newDir;
            NotifyObservers(SlugEvents.Turn);
        }
    }

    public void MoveForward() {
        if (!inTheAir) {
            absoluteVelocity.x = transform.right.x;
        }
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
        foreach(IObserver obs in observers) {
            obs.Observe(ev);
        }
    }
}

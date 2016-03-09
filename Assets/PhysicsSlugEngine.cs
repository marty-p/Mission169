using UnityEngine;

public class PhysicsSlugEngine : MonoBehaviour {

    private BoxCollider2D boxCollider;
    private IObserver[] observers;

    public float initialJumpVelocity = 3f;
    public float maxVerticalVelocity = -3;
    public float verticalDrag = 7f;
    private float currentVerticalVelocity = 0;

    private const float groundMovementFactor = 0.8f;
    private const float airLowVelocityMovementFactor = 1.1f;
    private const float airHighVelocityMovementFactor = 1.25f;
    private const float rayCastRestLength = 0.02f;
    private float movementFactor;
    
    private Vector2 absoluteVelocity;
    private Vector2 previousPos;

    private bool inTheAir = false;
    public bool InTheAir { get { return inTheAir;} }
    private float yCandidate;
    private Vector2 groundSlope;
    private Vector2 rayCastStartPoint;

    void Start () {
        boxCollider = GetComponent<BoxCollider2D>();
        rayCastStartPoint = new Vector2();
        observers = GetComponents<IObserver>();
        yCandidate = transform.position.y;
        absoluteVelocity = new Vector2();
        previousPos = new Vector2(transform.position.x, transform.position.y);
    }



    void FixedUpdate () 
    {
        RaycastHit2D hit = WhatIsUnderMyFeet();
        bool justFellFromPlatform = hit.collider == null && !inTheAir;
        bool justLanding = hit.collider != null;
        if ( justFellFromPlatform ) {
            StartFalling();
        } else if (justLanding) {
            StopFalling();
            yCandidate = FixYPosition(hit);
        } else if (inTheAir) {
            yCandidate += currentVerticalVelocity * Time.fixedDeltaTime;
        } else {
            yCandidate = transform.position.y;
        }

        transform.position = new Vector3(transform.position.x, yCandidate);

        UpdateVerticalVelocity();
        CalculateVelocity(Time.fixedDeltaTime);
        UpdateGroundSlope(hit);

        Debug.DrawLine(rayCastStartPoint, 
                new Vector2(boxCollider.bounds.min.x + boxCollider.bounds.size.x/2,
                boxCollider.bounds.min.y + currentVerticalVelocity * Time.fixedDeltaTime - rayCastRestLength));
    }

    RaycastHit2D WhatIsUnderMyFeet() {
        rayCastStartPoint = new Vector2(boxCollider.bounds.min.x + boxCollider.bounds.size.x/2,
                boxCollider.bounds.min.y - 0.0005f);
        Vector2 dir;
        if (currentVerticalVelocity > 0) {
            dir = Vector2.up;
        } else {
            dir = Vector2.down;
        }

        int layerMask = 1 << LayerMask.NameToLayer("World");

        return Physics2D.Raycast(rayCastStartPoint, dir,
                rayCastRestLength + Mathf.Abs(currentVerticalVelocity * Time.fixedDeltaTime), 
                layerMask);
    }


    float FixYPosition(RaycastHit2D hit) {
        return hit.point.y + boxCollider.bounds.size.y / 2 - boxCollider.offset.y + 0.01f;
    }

    void UpdateGroundSlope(RaycastHit2D hit) {
        if (inTheAir) {
            groundSlope.x = 1;
            groundSlope.y = 1;
            return;
        }

        Quaternion rotate = Quaternion.Euler(0, 0, -90 * transform.right.x);
        groundSlope = rotate * hit.normal;
        groundSlope.x = Mathf.Abs(groundSlope.x);

        groundSlope.Normalize();

        Debug.DrawLine(hit.point,
                new Vector2(hit.point.x + groundSlope.x,  hit.point.y + groundSlope.y), Color.cyan);
    }

    void UpdateVerticalVelocity() {
        if (inTheAir) {
            currentVerticalVelocity -= (verticalDrag*Time.fixedDeltaTime);
            Mathf.Clamp(currentVerticalVelocity, maxVerticalVelocity, initialJumpVelocity/3);
        } 
    }

    void StopFalling() {
        inTheAir = false;
        movementFactor = groundMovementFactor;
        currentVerticalVelocity = 0;
        NotifyObservers(SlugEvents.HitGround);
    }

    void StartFalling() {
        inTheAir = true;
        movementFactor = airLowVelocityMovementFactor;
        currentVerticalVelocity = 0;
        NotifyObservers(SlugEvents.Fall);
    }

    void CalculateVelocity(float dt) {
        absoluteVelocity.y = Mathf.Abs(transform.position.y - previousPos.y) / dt;
        absoluteVelocity.x = Mathf.Abs(transform.position.x - previousPos.x) / dt;
        previousPos.x = transform.position.x;
        previousPos.y = transform.position.y;
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
        currentVerticalVelocity = initialJumpVelocity;
    }

    public void changeDirection(Vector2 newDir) {
        transform.right = newDir;

    }

    public void MoveForward() {
        transform.Translate(groundSlope * movementFactor * Time.fixedDeltaTime);
        print(movementFactor);
    }

    void NotifyObservers(SlugEvents ev) {
        foreach(IObserver obs in observers) {
            obs.Observe(ev);
        }
    }
}

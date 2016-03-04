using UnityEngine;
using System.Collections;

public class PhysicsController : MonoBehaviour {

    private BoxCollider2D boxCollider;
    private IObserver[] observers;

    public float initialJumpVelocity = 5f;
    private float currentVerticalVelocity = 0;
    public float maxVerticalVelocity = -3;
    public float verticalDrag = 8f;
    public float fact = 1.5f;

    private const float groundMovementFactor = 1f;
    private const float airLowVelocityMovementFactor = 1.25f;
    private const float airHighVelocityMovementFactor = 1.5f;
    private float movementFactor;
    
    private Vector2 absoluteVelocity;
    private Vector2 previousPos;

    private bool inTheAir = false;
    public bool InTheAir { get { return inTheAir;} }
    private float yCandidate;
    private Vector2 rayCastStart;

    void Start () {
        boxCollider = GetComponent<BoxCollider2D>();
        rayCastStart = new Vector2();
        observers = GetComponents<IObserver>();
        yCandidate = transform.position.y;
        absoluteVelocity = new Vector2();
        previousPos = new Vector2(transform.position.x, transform.position.y);
    }



    void FixedUpdate () 
    {
        RaycastHit2D hit = WhatIsUnderMyFeet();
        bool justFellFromPlatform = hit.collider == null && !inTheAir;
        bool landing = hit.collider != null && hit.collider != boxCollider;
        if ( justFellFromPlatform ) {
            StartFalling();
        } else if ( landing ) {
            StopFalling();
            yCandidate = FixYPosition(hit);
        } else if ( inTheAir ) {
            yCandidate += currentVerticalVelocity * Time.fixedDeltaTime;
        } else {
            yCandidate = transform.position.y;
        }

        transform.position = new Vector3(transform.position.x, yCandidate);

        UpdateVerticalVelocity();
        CalculateVelocity(Time.fixedDeltaTime);

        //Debug.DrawLine(rayCastStart, 
        //        new Vector2( boxCollider.bounds.min.x + boxCollider.bounds.size.x/2,
        //        boxCollider.bounds.min.y + currentVerticalVelocity * Time.fixedDeltaTime - 0.005f )  );
    }

    RaycastHit2D WhatIsUnderMyFeet() {
        rayCastStart = new Vector2(boxCollider.bounds.min.x + boxCollider.bounds.size.x/2,
                boxCollider.bounds.min.y - 0.0005f);
        Vector2 dir;
        if (currentVerticalVelocity > 0) {
            dir = Vector2.up;
        } else {
            dir = Vector2.down;
        }
        return Physics2D.Raycast(rayCastStart, dir, 0.002f + Mathf.Abs(currentVerticalVelocity * Time.fixedDeltaTime));
    }


    float FixYPosition(RaycastHit2D hit) {
        return hit.collider.bounds.max.y + boxCollider.bounds.size.y / 2 - boxCollider.offset.y + 0.002f;
    }

    void UpdateVerticalVelocity() {
        if (inTheAir) {
            currentVerticalVelocity -= (verticalDrag*Time.fixedDeltaTime)*fact;
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

    void NotifyObservers(SlugEvents ev) {
        foreach(IObserver obs in observers) {
            obs.Observe(ev);
        }
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

    //TODO these 2 functions do the same thing
    public void MoveRight() {
        transform.Translate(Vector3.right * movementFactor * Time.fixedDeltaTime);
        NotifyObservers(SlugEvents.StartMoving);
    }

    public void MoveLeft() {
        transform.Translate(Vector3.right * movementFactor * Time.fixedDeltaTime);
        NotifyObservers(SlugEvents.StartMoving);
    }

}

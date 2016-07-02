using UnityEngine;

public class SlugPhysics : MonoBehaviour {

    private Collider2D boxCollider;
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

    private float Y;
    private float xTranslation;
    private float penteY;

    public bool debugging;
    public LayerMask linecastLayerMask;

    void Awake() {
        boxCollider = GetComponent<Collider2D>();
        rayCastStartPoint = new Vector2();
        observers = GetComponents<IObserver>();
        absoluteVelocity = new Vector2();
    }

    void myPrint(string str) {
        if (debugging) {
            print(str);
        }
    }

    void FixedUpdate() {

        CalculateVelocity();

        RaycastHit2D underMe = WhatIsUnderMyFeetRightNow();
        Vector2 currentSlope = GetSlopeFromRayCastHid2D(underMe);

        float dx, dy;
        if (InTheAir) {
            dx = absoluteVelocity.x * movementFactor * Time.fixedDeltaTime;
            dy = absoluteVelocity.y * Time.fixedDeltaTime;
            myPrint("inTheAir yo");
        } else {
            dx = absoluteVelocity.x * movementFactor * Time.fixedDeltaTime * Mathf.Abs(currentSlope.x);
            dy = Mathf.Abs(absoluteVelocity.x) * movementFactor * Time.fixedDeltaTime * currentSlope.y;
            myPrint("on the ground yo " + dx +"/"+dy);
        }

        Vector2 transCandidate = new Vector2(dx, dy);

        RaycastHit2D inFront = WhatIsInFrontOfMe(transCandidate);
        Vector2 facingWallslope = GetSlopeFromRayCastHid2D(inFront);
        if (inFront.collider != null && Mathf.Abs(facingWallslope.y) > 0.85f) {
            transCandidate.x = 0;
            dx = 0;
        }

        RaycastHit2D underMeFutur = WhatIsUnderMyFeet(transCandidate);
        if (underMeFutur.collider != null) {
            myPrint(" collision detection " + inTheAir + " " + absoluteVelocity.y);
            if (inTheAir && absoluteVelocity.y > 0) {
                // rien
            }else if (inTheAir && absoluteVelocity.y < 0) {
                StopFalling();
                dy = FixYTrans(underMeFutur);
            } else {
                myPrint("On THE GROUND");
                dy = FixYTrans(underMeFutur);
            }
        } else if (underMeFutur.collider == null && !inTheAir) {
            StartFalling();
            myPrint("start falling");
        }

        transform.Translate(dx, dy, 0, Space.World);
    }

    float calculateXTranslation() {
        //float xTranslation = absoluteVelocity.x * movementFactor * Time.fixedDeltaTime;
        //float penteX = 1 - Mathf.Abs(penteY);
        float penteX = Mathf.Abs(groundSlope.x);
        if (InTheAir) {
            xTranslation = absoluteVelocity.x * movementFactor * Time.fixedDeltaTime;
        } else {
            xTranslation = penteX * absoluteVelocity.x * movementFactor * Time.fixedDeltaTime;
        }

        if (groundSlope.y > 0.85f) {
                xTranslation = 0;
                print(" YO");
        }

            //print("Xtranslation " + penteX);
        return xTranslation;
    }

    float calculateYTranslation() {
        float yTranslation = 0;
        bool justFellFromPlatform = underMyFeetHit.collider == null
                && inFrontOfMe.collider == null
                && !inTheAir;
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
            yTranslation = FixYTrans(underMyFeetHit);
        } else if (inTheAir) {
            yTranslation = absoluteVelocity.y * Time.fixedDeltaTime;
        } else {
            // I may be going up/down a hill
            //yTranslation = Mathf.Abs(absoluteVelocity.x) * movementFactor * Time.fixedDeltaTime * penteY;
            if (groundSlope.y > 0.85f) {
                yTranslation = 0;
                print("YEQQQQQQQQQQQQQQQQQQQQQQH");
            } else {
                yTranslation = Mathf.Abs(absoluteVelocity.x) * movementFactor * Time.fixedDeltaTime * groundSlope.y + FixYTrans(underMyFeetHit);
                //yTranslation = Mathf.Abs(absoluteVelocity.x) * movementFactor * Time.fixedDeltaTime * groundSlope.y ;
            }
        }
        return yTranslation;
    }

    RaycastHit2D  WhatIsUnderMyFeet(Vector2 trans) {
        float WHAT_NAME = 0.05f;
        Vector2 startPoint = new Vector2(boxCollider.bounds.center.x + trans.x, boxCollider.bounds.min.y + trans.y - WHAT_NAME);
        Vector2 endPoint = new Vector2(startPoint.x, startPoint.y + WHAT_NAME*2);

        RaycastHit2D under = Physics2D.Linecast(endPoint, startPoint, linecastLayerMask);
        myPrint("futur " + under.collider);
        Debug.DrawLine(endPoint, startPoint);
        return under;
    }

    RaycastHit2D WhatIsInFrontOfMe(Vector2 trans) {
        Bounds bounds = boxCollider.bounds;
        float startX = bounds.center.x + transform.right.x * bounds.size.x / 2;

        float safety = 0.02f * transform.right.x;
        if (Mathf.Abs(trans.x) > safety) {
            safety = 0;
        }
        safety = 0;

        Vector2 startPoint = new Vector2(startX, bounds.max.y);
        Vector2 endPoint = startPoint + new Vector2(trans.x + safety, trans.y);

        inFrontOfMe = Physics2D.Linecast(startPoint, endPoint, linecastLayerMask);

        if (inFrontOfMe.collider == null) {
            startPoint = new Vector2(startX, bounds.min.y);
            endPoint = startPoint + new Vector2(trans.x+safety, trans.y);
            inFrontOfMe = Physics2D.Linecast(startPoint, endPoint, linecastLayerMask);
        }
        return inFrontOfMe;
    }

    RaycastHit2D WhatIsUnderMyFeetRightNow() {
        float dy = -0.2f;
        rayCastStartPoint = new Vector2(boxCollider.bounds.center.x, boxCollider.bounds.min.y + 0.1f);
        Vector2 endPoint = new Vector2(rayCastStartPoint.x, rayCastStartPoint.y + dy);

        return Physics2D.Linecast(rayCastStartPoint, endPoint, linecastLayerMask);
    }

    float FixYTrans(RaycastHit2D hit) {
        return hit.point.y - boxCollider.bounds.min.y + 0.005f;

    }

    Vector2 GetSlopeFromRayCastHid2D(RaycastHit2D hit) {
        Quaternion rotate = Quaternion.Euler(0, 0, -90 * transform.right.x);

        Vector2 slope = rotate * hit.normal;
        slope.Normalize(); //Is it really necessary to normalize
        return slope;
/*
        Debug.DrawLine(underMyFeetHit.point,
                        new Vector2(underMyFeetHit.point.x + groundSlope.x,  underMyFeetHit.point.y + groundSlope.y),
                        Color.cyan);
*/
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
            absoluteVelocity.y -= (verticalDrag * Time.fixedDeltaTime);
            Mathf.Clamp(absoluteVelocity.y, maxVerticalVelocity, initialJumpVelocity / 3);
        } else {
            absoluteVelocity.x =  absoluteVelocity.x * groundDrag + forceX;
        }

            print("abs Vel:" + " " + absoluteVelocity.x);
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
            //groundSlope = Vector2.zero;
           // UpdateGroundSlope();
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

using System.Collections;
using UnityEngine;
using Utils;

public class SlugPhysics : MonoBehaviour {

    private Collider2D collider;
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
    private const float maxSlope = 0.8f;


    private Vector2 absoluteVelocity;

    private RaycastHit2D[] rayCastHit = new RaycastHit2D[1];
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
        collider = GetComponent<Collider2D>();
        rayCastStartPoint = new Vector2();
        observers = GetComponents<IObserver>();
        absoluteVelocity = new Vector2();
    }

    void myPrint(string str) {
        if (debugging) {
            print(str);
        }
    }

    void FixedUpdate() {;
        
        // 1 - Update velocities
        CalculateVelocity();
        
        // 2 - Calculate translations depending on current current slope 
        Vector2 groundSlope;
        if (WhatIsUnderMyFeet(Vector2.zero) > 0) {
            groundSlope = GetSlopeFromRayCastHid2D(rayCastHit[0]);
        } else {
            groundSlope = Vector2.zero;
        }
        Vector2 transCandidate = CalculateTranslation(groundSlope);

        // 3 - Adjust translations if collisions next frame
        // 3.1 - Adjust x
        if (WhatIsInFrontOfMe(transCandidate) > 0) {
            Vector2 facingWallslope = GetSlopeFromRayCastHid2D(rayCastHit[0]);
            if (Mathf.Abs(facingWallslope.y) > maxSlope) {
                transCandidate.x = FixXTrans(rayCastHit[0]);
                if (!inTheAir) {
                    transCandidate = Vector2.zero;
                    goto TRANSLATE;
                }
            }
        }
        // 3.2 Adjust y
        if (WhatIsUnderMyFeet(transCandidate) > 0) {
            Vector2 futurUnderslope = GetSlopeFromRayCastHid2D(rayCastHit[0]);
            if (inTheAir) {
                if (absoluteVelocity.y < 0) { // landing
                    if (Mathf.Abs(futurUnderslope.y) < maxSlope) {
                        StopFalling();
                    }
                    transCandidate.y = FixYTrans(rayCastHit[0]);
                }
            } else {
                transCandidate.y = FixYTrans(rayCastHit[0]); // just following the curve of the ground
            }
        } else if (!inTheAir) {
            StartFalling();
        }

        TRANSLATE:
        transform.Translate(transCandidate.x, transCandidate.y, 0, Space.World);
    }


    Vector2 CalculateTranslation(Vector2 groundSlope) {
        Vector2 trans = new Vector2();
        if (inTheAir && groundSlope != Vector2.zero && absoluteVelocity.y < 0) { // sliding on a steep slope
            trans.x = 1.8f * Time.fixedDeltaTime * Mathf.Abs(groundSlope.x);
            trans.y = -1.8f * Time.fixedDeltaTime * Mathf.Abs(groundSlope.y) ;
        } else if (InTheAir) { // falling or ascending
            trans.x = absoluteVelocity.x * movementFactor * Time.fixedDeltaTime;
            trans.y = absoluteVelocity.y * Time.fixedDeltaTime;
        } else { // On the ground
            trans.x = absoluteVelocity.x * movementFactor * Time.fixedDeltaTime * Mathf.Abs(groundSlope.x);
            trans.y = Mathf.Abs(absoluteVelocity.x) * movementFactor * Time.fixedDeltaTime * groundSlope.y;
        }
        return trans;
    }


    int WhatIsUnderMyFeet(Vector2 trans) {
        Vector2 endPoint = new Vector2(collider.bounds.center.x + trans.x, collider.bounds.min.y + trans.y - 0.03f);
        Vector2 startPoint = new Vector2(endPoint.x, collider.bounds.min.y + 0.03f);
        //Only supporting one hit per cast, no need for more for now
        int hitCount = Physics2D.LinecastNonAlloc(startPoint, endPoint, rayCastHit, linecastLayerMask);
        //Debug.DrawLine(startPoint, endPoint);
        return hitCount;
    }

    int WhatIsInFrontOfMe(Vector2 trans) {
        Bounds bounds = collider.bounds;
        float startX = bounds.center.x;

        Vector2 startPoint = new Vector2(startX, bounds.min.y);
        Vector2 endPoint = startPoint + new Vector2(trans.x, trans.y);

        int hitCount = Physics2D.LinecastNonAlloc(startPoint, endPoint, rayCastHit, linecastLayerMask);
        Debug.DrawLine(startPoint, endPoint);
        if (hitCount == 0) {
            startPoint = new Vector2(startX, bounds.max.y);
            endPoint = startPoint + new Vector2(trans.x, trans.y);
            hitCount = Physics2D.LinecastNonAlloc(startPoint, endPoint, rayCastHit, linecastLayerMask);
        }
        return hitCount;
    }

    float FixXTrans(RaycastHit2D hit) {
        if (transform.right == Vector3.left) {
            return rayCastHit[0].point.x - collider.bounds.center.x + 0.03f;
        } else {
            return rayCastHit[0].point.x - collider.bounds.center.x - 0.03f;
        }
    }

    float FixYTrans(RaycastHit2D hit) {
        return hit.point.y - collider.bounds.min.y + 0.005f;
    }

    Vector2 GetSlopeFromRayCastHid2D(RaycastHit2D hit) {
        Quaternion rotate = Quaternion.Euler(0, 0, -90 * transform.right.x);
        Vector2 slope = rotate * hit.normal;
        return slope;
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
        StartCoroutine(WaitForPhysUpdate( ()=> {
            absoluteVelocity.x = velX * transform.right.x;
            absoluteVelocity.y = velY * transform.up.y;
        }));
    }

    public void SetVelocityX(float velX) {
        StartCoroutine(WaitForPhysUpdate( ()=> {
            absoluteVelocity.x = velX;
        }));
    }

    public void SetVelocityY(float velY) {
        StartCoroutine(WaitForPhysUpdate( ()=> {
            absoluteVelocity.y = velY;
            //FIXME
            inTheAir = true;
        }));
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

    void NotifyObservers(SlugEvents ev) {
        if (observers == null) {
            return;
        }
        foreach (IObserver obs in observers) {
            obs.Observe(ev);
        }
    }

    private IEnumerator WaitForPhysUpdate(RetVoidTakeVoid cb) {
        yield return new WaitForFixedUpdate();
        cb();
    }

}

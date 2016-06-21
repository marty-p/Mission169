using UnityEngine;

public class InputManager : MonoBehaviour {
    private IObserver[] observers;
    private MovementManager movementManager;
    private MarcoAttackManager attackManager;

    private bool sitting;
    private bool lookingUp;
    private bool lookingDown;

    void Start() {
        observers = GetComponentsInChildren<IObserver>();
        movementManager = GetComponent<MovementManager>();
        attackManager = GetComponentInChildren<MarcoAttackManager>();
    }

    void Update () {
        bool jump;
        bool fireKey;
        bool grenadeKey;

        bool leftKeyDown;
        bool leftKeyUp;
        bool rightKeyDown;
        bool rightKeyUp;
        bool upKeyDown;
        bool upKeyUp;
        bool downKeyDown;
        bool downKeyUp;

        int horizontalKey = 0;

        jump = Input.GetButtonDown("Jump");
        fireKey = Input.GetButtonDown("Fire1");
        grenadeKey = Input.GetButtonDown("Fire2");

        leftKeyDown = Input.GetKeyDown("left");
        leftKeyUp = Input.GetKeyUp("left");
        rightKeyDown = Input.GetKeyDown("right");
        rightKeyUp = Input.GetKeyUp("right");
        upKeyDown = Input.GetKeyDown("up");
        upKeyUp = Input.GetKeyUp("up");
        downKeyDown = Input.GetKeyDown("down");
        downKeyUp = Input.GetKeyUp("down");

        horizontalKey = (int) Input.GetAxisRaw("Horizontal");

        if (fireKey) {
            attackManager.PrimaryAttack();
        } else if (grenadeKey) {
            attackManager.SecondaryAttack();
        }

        if (leftKeyDown) {
            movementManager.HorizontalMovement(Vector3.left);
        } else if (leftKeyUp  && horizontalKey == 0) { //fix problem when changing direction (press right before releasing left)
            movementManager.StopMoving();
        } else if (rightKeyDown) {
            movementManager.HorizontalMovement(Vector3.right);
        } else if(rightKeyUp && horizontalKey == 0) {
            movementManager.StopMoving();
        }

        if (upKeyDown) {
            movementManager.LookUp();
        } else if (upKeyUp) {
            movementManager.DefaultBodyPosition();
        } else if (downKeyDown) {
            movementManager.DownMovement();
        } else if(downKeyUp) {
            movementManager.DefaultBodyPosition();
        }

        if (jump) {
            movementManager.Jump();
        }

    }

    void NotifyObservers(SlugEvents ev) {
        foreach(IObserver obs in observers) {
            obs.Observe(ev);
        }
    }
}

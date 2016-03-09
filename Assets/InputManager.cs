using UnityEngine;

public class InputManager : MonoBehaviour {

    private IObserver[] observers;
    private PhysicsSlugEngine physics;
    private bool fireKeyLocked = false;
    private bool jumpKeyLocked = false;

    private Vector3 faceRight = new Vector3(0, 0, 0);
    private Vector3 faceLeft = new Vector3(0, 180, 0);

    void Start() {
        observers = GetComponentsInChildren<IObserver>();
        physics = GetComponent<PhysicsSlugEngine>();
    }

    void FixedUpdate () {
        int horizontalKey = 0;
        bool jump;
        int downKey = 0;
        bool fireKey;

        jump = Input.GetButtonDown("Jump");
        horizontalKey = (int) Input.GetAxisRaw("Horizontal");
        downKey = (int)Input.GetAxisRaw("Vertical");
        fireKey = Input.GetButtonDown("Fire1");

        if (jump) {
            physics.Jump();
        }

        if (fireKey) {
            NotifyObservers(SlugEvents.Attack);
        }

        if (horizontalKey == 1) {
            if (transform.right == Vector3.left && !physics.InTheAir && downKey == 0 ) {
                NotifyObservers(SlugEvents.Turn);
            }
            physics.changeDirection(Vector2.right);
            physics.MoveForward();
            NotifyObservers(SlugEvents.GoingRight);
            NotifyObservers(SlugEvents.StartMoving);
        } else if (horizontalKey == -1) {
            if (transform.right == Vector3.right && !physics.InTheAir && downKey == 0) {
                NotifyObservers(SlugEvents.Turn);
            }
            physics.changeDirection(Vector2.left);
            physics.MoveForward();
            NotifyObservers(SlugEvents.GoingLeft);
            NotifyObservers(SlugEvents.StartMoving);
        } else if (horizontalKey == 0) {
            NotifyObservers(SlugEvents.StopMoving);
        }

        if (downKey == -1) {
            NotifyObservers(SlugEvents.Sit);
        } else if(downKey == 1) {
            NotifyObservers(SlugEvents.LookUp);
        } else if (downKey == 0) {
            NotifyObservers(SlugEvents.Stand);
        }
    }

    void NotifyObservers(SlugEvents ev) {
        foreach(IObserver obs in observers) {
            obs.Observe(ev);
        }
    }
}

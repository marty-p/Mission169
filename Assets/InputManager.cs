using UnityEngine;

public class InputManager : MonoBehaviour {

    private IObserver[] observers;
    public PhysicsController physicsController;
    private bool fireKeyLocked = false;
    private bool jumpKeyLocked = false;

    private Vector3 faceRight = new Vector3(0, 0, 0);
    private Vector3 faceLeft = new Vector3(0, 180, 0);

    void Start() {
        observers = GetComponentsInChildren<IObserver>();
    }

    void FixedUpdate () {
        int right_key = 0;
        int jump = 0;
        int downKey = 0;
        int fireKey = 0;

        jump = (int)Input.GetAxisRaw("Jump");
        right_key = (int) Input.GetAxisRaw("Horizontal");
        downKey = (int)Input.GetAxisRaw("Vertical");
        fireKey = (int)(Input.GetAxisRaw("Fire1"));
       
        if (jump == 1 && !jumpKeyLocked) {
            physicsController.Jump();
            jumpKeyLocked = true;
        } else if (jump == 0) {
            jumpKeyLocked = false;
        }

        if (fireKey == 1 && !fireKeyLocked) {
            NotifyObservers(SlugEvents.Attack);
            fireKeyLocked = true;
        }  else if (fireKey == 0) {
            fireKeyLocked = false;
        }

        if (right_key == 1) {
            if ( transform.right == Vector3.left && !physicsController.InTheAir && downKey == 0 ) {
                NotifyObservers(SlugEvents.Turn);
            }
            NotifyObservers(SlugEvents.IsMoving);
            transform.eulerAngles = faceRight;
            physicsController.MoveRight();
        } else if (right_key == -1) {
            if ( transform.right == Vector3.right && !physicsController.InTheAir && downKey == 0) {
                NotifyObservers(SlugEvents.Turn);
            }
            NotifyObservers(SlugEvents.IsMoving);
            transform.eulerAngles = faceLeft;
            physicsController.MoveLeft();
        } else if (right_key == 0) {
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

using UnityEngine;

public class InputManager : MonoBehaviour {
    private IObserver[] observers;
    private PhysicsSlugEngine physics;

    void Start() {
        observers = GetComponentsInChildren<IObserver>();
        //physics = GetComponent<PhysicsSlugEngine>();
    }

    void Update () {
        bool jump;
        bool fireKey;
        bool grenadeKey;
        int verticalKey = 0;
        int horizontalKey = 0;

        jump = Input.GetButtonDown("Jump");
        fireKey = Input.GetButtonDown("Fire1");
        grenadeKey = Input.GetButtonDown("Fire2");
        horizontalKey = (int) Input.GetAxisRaw("Horizontal");
        verticalKey = (int)Input.GetAxisRaw("Vertical");

        if (fireKey) {
            NotifyObservers(SlugEvents.Attack);
        } else if (grenadeKey) {
            NotifyObservers(SlugEvents.Grenade);
        }
        if (horizontalKey == 1) {
            NotifyObservers(SlugEvents.MovingRight);
        } else if (horizontalKey == -1) {
            NotifyObservers(SlugEvents.MovingLeft);
        } else if (horizontalKey == 0) {
            NotifyObservers(SlugEvents.StopMoving);
        }
        if (verticalKey == -1) {
            NotifyObservers(SlugEvents.Sit);
        } else if(verticalKey == 1) {
            NotifyObservers(SlugEvents.LookUp);
        } else if (verticalKey == 0) {
            NotifyObservers(SlugEvents.Stand);
        }
        
        // TODO if jump is done before right or left key then
        // we can't jump and have a momentum, this sounds wrong and should
        // be fixed? 
        if (jump) {
            NotifyObservers(SlugEvents.Jump);
        }

    }

    void NotifyObservers(SlugEvents ev) {
        foreach(IObserver obs in observers) {
            obs.Observe(ev);
        }
    }
}

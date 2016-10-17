using UnityEngine;
#if UNITY_ANDROID || UNITY_IOS
    using CnControls;
#endif

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

        int horizontalKey = 0;
        int verticalKey = 0;

#if UNITY_ANDROID || UNITY_IOS
        jump = CnInputManager.GetButtonDown("Jump");
        fireKey = CnInputManager.GetButtonDown("Fire1");
        grenadeKey = CnInputManager.GetButtonDown("Fire2");
        horizontalKey = (int) CnInputManager.GetAxisRaw("Horizontal");
        verticalKey = (int) CnInputManager.GetAxis("Vertical");
#else
        jump = Input.GetButtonDown("Jump");
        fireKey = Input.GetButtonDown("Fire1");
        grenadeKey = Input.GetButtonDown("Fire2");
        horizontalKey = (int) Input.GetAxisRaw("Horizontal");
        verticalKey = (int)Input.GetAxis("Vertical");
#endif

        if (fireKey) {
            attackManager.PrimaryAttack();
        } else if (grenadeKey) {
            attackManager.SecondaryAttack();
        }

        if (horizontalKey < 0) {
            movementManager.HorizontalMovement(Vector3.left);
        } else if (horizontalKey == 0) { //fix problem when changing direction (press right before releasing left)
            movementManager.StopMoving();
        } else if (horizontalKey > 0) {
            movementManager.HorizontalMovement(Vector3.right);
        }

        if (verticalKey > 0) {
            movementManager.LookUp();
        } else if (verticalKey == 0) {
            movementManager.DefaultBodyPosition();
        } else if (verticalKey < 0) {
            movementManager.DownMovement();
        }

        if (jump) {
            movementManager.Jump();
        }

        if (Input.anyKeyDown) {
            CancelInvoke();
            InvokeRepeating("SendPlayerInactiveEvent", 5, 2);
        }

    }

    void NotifyObservers(SlugEvents ev) {
        foreach(IObserver obs in observers) {
            obs.Observe(ev);
        }
    }

    void SendPlayerInactiveEvent() {
        EventManager.Instance.TriggerEvent(GlobalEvents.PlayerInactive);
    }
}

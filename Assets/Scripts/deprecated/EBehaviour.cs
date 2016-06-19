using UnityEngine;

public abstract class EBehaviour : MonoBehaviour {

    public bool interruptible = true;
    public int priority = 1;
    public bool exclusive = false;
    protected bool resting;
    public bool Resting { get{ return resting;}}
    public float restingTime = 1;

    public abstract bool WantToStart();
}

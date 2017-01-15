using UnityEngine;

public abstract class EnemyBrain : MonoBehaviour {

    public virtual void Pause() {
        enabled = false;
    }

    public virtual void Reset() {
        enabled = true;
    }

}

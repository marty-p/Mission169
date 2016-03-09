using UnityEngine;
using System;

public class MovementManager : MonoBehaviour, IObserver {
    private PhysicsSlugEngine physics;

    public void Observe(SlugEvents ev) {

        if (true) {
            return;
        }

        if (ev == SlugEvents.GoingRight) {
            physics.changeDirection(Vector2.right);
            physics.MoveForward();
        } else if ( ev == SlugEvents.GoingLeft) {
            physics.changeDirection(Vector2.left);
            physics.MoveForward();
        }
    }

    // Use this for initialization
    void Start () {
        physics = GetComponent<PhysicsSlugEngine>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

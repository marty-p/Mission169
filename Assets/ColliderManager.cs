using UnityEngine;
using System.Collections;
using System;

public class ColliderManager : MonoBehaviour, IObserver {

    private BoxCollider2D collider;
    private Vector2 standingSize;
    private Vector2 standingOffset;
    private Vector2 sittingSize = new Vector2(0.21f, 0.18f);
    private Vector2 sittingOffset = new Vector2 (0, 0.01f);

    public void Observe(SlugEvents ev) {
        if (ev == SlugEvents.Sit) {
            //collider.size = sittingSize;
            //collider.offset = sittingOffset;
        } else if (ev == SlugEvents.Stand) {
            collider.size = standingSize;
            collider.offset = standingOffset;
        }
    }

    void Start () {
        collider = GetComponent<BoxCollider2D>();
        standingSize = collider.size;
        standingOffset = collider.offset;
	}
	
}

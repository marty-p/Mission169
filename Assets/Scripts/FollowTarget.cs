using UnityEngine;
using System.Collections;

public class FollowTarget : MonoBehaviour {

    private SpriteRenderer pointOfScrolling;
    public Transform target;
    public bool followActive = true;
    private new Camera camera;
    private Vector2 targetViewPortPos;

    private Vector2 oldTargetPosition;


    void Start () {
        camera = GetComponent<Camera>();
        targetViewPortPos = new Vector2();
        oldTargetPosition = new Vector2(target.transform.position.x, target.transform.position.y);
    }
	
	void FixedUpdate () {
        targetViewPortPos = camera.WorldToViewportPoint(target.position);
        if (followActive) {
            Follow();
        }
        KeepTargetInSight();
    }

    void Follow() {
        if (targetViewPortPos.x > 0.5F) {
            Vector3 dest = new Vector3(target.transform.position.x + 0.25f, transform.position.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, dest, 3 * Time.deltaTime);
        }
    }

    void KeepTargetInSight() {
        if (targetViewPortPos.x  < 0.03f ||  targetViewPortPos.x > 1 - 0.03f ) {
            target.transform.position = new Vector2(oldTargetPosition.x, target.transform.position.y);
        }

        oldTargetPosition = target.transform.position;
    }

}

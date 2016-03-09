using UnityEngine;
using System.Collections;

public class FollowTarget : MonoBehaviour {

    private SpriteRenderer pointOfScrolling;
    public Transform target;
    private Camera camera;


	void Start () {
        camera = GetComponent<Camera>();
    }
	
	void Update () {
        Vector3 targetViewPortPos = camera.WorldToViewportPoint(target.position);
        if (targetViewPortPos.x > 0.3F) {
            Vector3 dest = new Vector3(target.transform.position.x + 0.8f, transform.position.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, dest, 3 * Time.deltaTime);
        }
            
    }
}

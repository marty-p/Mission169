using UnityEngine;
using System.Collections;

public class InfiiniteScrolling : MonoBehaviour {

    public SpriteRenderer bit1;
    public SpriteRenderer bit2;
    public Camera cam;
    public float scrollingFact = 0.025f;

	void Update () {
        bit1.transform.transform.Translate(-Time.deltaTime * scrollingFact, 0, 0);
        bit2.transform.transform.Translate(-Time.deltaTime * scrollingFact, 0, 0);

        if (!bit1.isVisible) {
            bit1.transform.position = bit2.bounds.center + new Vector3(bit2.bounds.size.x, 0, 0);
        }
        if (!bit2.isVisible) {
            bit2.transform.position = bit1.bounds.center + new Vector3(bit1.bounds.size.x, 0, 0);
        }

        if (!bit1.isVisible && !bit2.isVisible) {
            bit1.transform.position = new Vector3(cam.transform.position.x, bit1.transform.position.y, 0);
            bit2.transform.position = bit1.bounds.center + new Vector3(bit1.bounds.size.x, 0, 0);
        }

	}
}

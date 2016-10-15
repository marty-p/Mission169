using UnityEngine;
using System.Collections;

public class InfiiniteScrolling : MonoBehaviour {

    //TODO have an array of 'bits', would come handy if scrolling more than 2 sprites
    public SpriteRenderer bit1;
    public SpriteRenderer bit2;
    public Camera cam;
    public float scrollingFact = 0.025f;

	void Update () {
        float transX = -Time.deltaTime * scrollingFact;
        transform.transform.Translate(transX, 0, 0);

        // went off screen, the sprite is positioned after the other one
        if (!bit1.isVisible) {
            bit1.transform.position = bit2.bounds.center + new Vector3(bit2.bounds.size.x, 0, 0);
        }
        if (!bit2.isVisible) {
            bit2.transform.position = bit1.bounds.center + new Vector3(bit1.bounds.size.x, 0, 0);
        }

        if (!bit1.isVisible && !bit2.isVisible) {
            bit2.transform.position = bit1.bounds.center + new Vector3(bit1.bounds.size.x, 0, 0);
            bit1.transform.position = new Vector3(cam.transform.position.x, bit1.transform.position.y, 0);
        }

	}
}

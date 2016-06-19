using UnityEngine;
using System.Collections;

public class FlipPlayerIndicator : MonoBehaviour {

    private SpriteRenderer sprite;

    void Start () {
        sprite = GetComponent<SpriteRenderer>();
	}

	void Update () {
	    if (transform.right.x == -1) {
            sprite.flipX = true;
        } else {
            sprite.flipX = false;
        }
	}

    public void SetVisible(bool visible) {
        gameObject.SetActive(visible);
    }
}

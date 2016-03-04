using UnityEngine;

public class SimulateHovering : MonoBehaviour {

    private Vector2 groundBoxCollider;
    private BoxCollider2D boxCollider;
    private Vector2 groundOffset;
    public bool active = true;
    private Vector3[] childsBasePos;
    private Transform[] childs;
    public SpriteRenderer bottomSpriteRenderer;

    public void hoveringOn () {
        int i = 0;
        foreach(Transform t in childs) {
            if (i == 0) {
                i++;
                continue;
            }
            Vector3 dest = new Vector3(childsBasePos[i].x, childsBasePos[i].y + 0.035f, childsBasePos[i].z);
            t.localPosition = dest;
            i++;
        }
    }

    public void hoveringOff () {
        int i = 0;
        foreach(Transform t in childs) {
            if (i == 0) {
                i++;
                continue;

            }
            t.localPosition = childsBasePos[i];
            i++;
        }
    }

    void Start () {
        boxCollider = GetComponent<BoxCollider2D>();
        groundBoxCollider = boxCollider.size;
        groundOffset = boxCollider.offset;
        childs = GetComponentsInChildren<Transform>();
        int i = 0;
        childsBasePos = new Vector3[4];
        foreach(Transform t in childs) {
            if (i == 0) {
                i++;
                continue;
            }
            childsBasePos[i] = t.localPosition;
            i++;
        }
	}
	
    public void HideBottom() {
        bottomSpriteRenderer.enabled = false;
        print("hidden");
    }

    public void ShowBottom() {
        bottomSpriteRenderer.enabled = true;
        print("shown");
    }
}

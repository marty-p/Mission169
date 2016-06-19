using UnityEngine;

public class SimulateHovering : MonoBehaviour {

    private Vector3[] childInitialPos;
    private Transform[] childs;
    public SpriteRenderer bottomSpriteRenderer;
    private Vector2 childDestPos;

    public void hoveringOn () {
        for (int i=0; i < childs.Length; i++) {
            if (i == 0) {
                continue;
            }
            float newLocalPos = childInitialPos[i].y + 0.035f;
            childDestPos.Set(childInitialPos[i].x, newLocalPos);
            childs[i].localPosition = childDestPos;
        }
    }

    public void hoveringOff () {
        for (int i=0; i < childs.Length; i++) {
            if (i == 0) {
                continue;
            }
            childs[i].localPosition  = childInitialPos[i] ;
        }
    }

    void Start () {
        childs = GetComponentsInChildren<Transform>();
        childDestPos = new Vector2();
        childInitialPos = new Vector3[childs.Length];
        for (int i=0; i < childs.Length; i++) {
            if (i == 0) {
                continue;
            }
            childInitialPos[i] = childs[i].localPosition;
        }
	}
	
    public void HideBottom() {
        bottomSpriteRenderer.enabled = false;
    }

    public void ShowBottom() {
        bottomSpriteRenderer.enabled = true;
    }
}

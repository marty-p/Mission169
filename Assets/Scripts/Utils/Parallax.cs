using UnityEngine;

[System.Serializable]
public struct BgLayer {
    public GameObject go;
    public float scrollingRatioToForeground;
}

public class Parallax : MonoBehaviour {

    public BgLayer[] bgLayers;
    private Vector2 oldPos;

    void Start() {
        oldPos = transform.position;
    }

    void FixedUpdate () {
        Vector2 camTrans = (Vector2) transform.position - oldPos;

        if (camTrans != Vector2.zero) {
            for (int i=0; i<bgLayers.Length; i++){
                bgLayers[i].go.transform.Translate(
                        (1-bgLayers[i].scrollingRatioToForeground)*camTrans);
            }
        }
        oldPos = transform.position;
	}
}

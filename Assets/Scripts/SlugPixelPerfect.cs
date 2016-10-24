using UnityEngine;

public class SlugPixelPerfect : MonoBehaviour {

	void Start () {
        Resolution res = Screen.currentResolution;
        float aspectRatio = (float)res.width / res.height;
        if (aspectRatio < 1.5f) {
            GetComponent<PixelPerfectCamera>().pixelPerfect = false;
        }
    }
	
}

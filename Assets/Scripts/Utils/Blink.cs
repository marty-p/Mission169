using UnityEngine;
using System.Collections;
using Utils;


public class Blink : MonoBehaviour {

    private SpriteRenderer[] spriteRenderers;
    private bool[] blinkSprite;

	void Start () {
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        blinkSprite = new bool[spriteRenderers.Length];
	}
	
    public void BlinkPlease(RetVoidTakeVoid cb) {
        // if the sprite is initially hidden we will disregard it during the blinking
        for (int i=0; i< spriteRenderers.Length; i++) {
            blinkSprite[i] = spriteRenderers[i].enabled;
        }
        StartCoroutine("BlinkCoroutine", cb);
    }

    private IEnumerator BlinkCoroutine(RetVoidTakeVoid cb) {
        int blinkCount = 0;
        int blinkTotal = 25;
        while (blinkCount < blinkTotal) {
            for (int i=0; i< spriteRenderers.Length; i++) {
                if (blinkSprite[i]) {
                    spriteRenderers[i].enabled = !spriteRenderers[i].enabled;
                }
            }
            blinkCount++;
            yield return new WaitForSeconds(0.04f);
        }
        for (int i=0; i< spriteRenderers.Length; i++) {
            spriteRenderers[i].enabled = true;
        }
        if (cb != null) {
            cb();
        }
    }
}

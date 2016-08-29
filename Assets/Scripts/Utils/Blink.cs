using UnityEngine;
using System.Collections;
using Utils;

// TODO
// Does BLink have to be a MonoBeahviour?

public class Blink : MonoBehaviour {

    private const float blinkDuration = 1.25f;
    private const float switchDuration = 0.025f;
    private float elapsedTime = 0;
    private float elapsedSwitch = 0;
    private SpriteRenderer[] spriteRenderers;
    private bool[] blinkSprite;

	void Start () {
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        blinkSprite = new bool[spriteRenderers.Length];
	}
	
    public void BlinkPlease(RetVoidTakeVoid cb) {
        elapsedTime = 0;
        elapsedSwitch = 0;
        // if the sprite is initially hidden we will disregard it during the blinking
        for (int i=0; i< spriteRenderers.Length; i++) {
            blinkSprite[i] = spriteRenderers[i].enabled;
        }
        StartCoroutine("BlinkCoroutine", cb);
    }

    private IEnumerator BlinkCoroutine(RetVoidTakeVoid cb) {
        while (elapsedTime < blinkDuration) {
            if (elapsedSwitch >= switchDuration) {
                for (int i=0; i< spriteRenderers.Length; i++) {
                    if (blinkSprite[i]) {
                        spriteRenderers[i].enabled = !spriteRenderers[i].enabled;
                    }
                }
                elapsedSwitch = 0;
            }
            elapsedSwitch += Time.deltaTime;

            elapsedTime += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        for (int i=0; i< spriteRenderers.Length; i++) {
            spriteRenderers[i].enabled = true;
        }
        if (cb != null) {
            cb();
        }
    }
}

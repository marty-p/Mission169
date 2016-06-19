using UnityEngine;
using System.Collections;
using Utils;

// TODO
// Does BLink have to be a MonoBeahviour?

public class Blink : MonoBehaviour {

    private const float blinkDuration = 1.5f;
    private const float switchDuration = 0.04f;
    private float elapsedTime = 0;
    private float elapsedSwitch = 0;
    private SpriteRenderer[] spriteRenderers;

	void Start () {
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
	}
	
    public void BlinkPlease(RetVoidTakeVoid cb) {
        elapsedTime = 0;
        elapsedSwitch = 0;
        StartCoroutine("BlinkCoroutine", cb);
    }

    private IEnumerator BlinkCoroutine(RetVoidTakeVoid cb) {
        while (elapsedTime < blinkDuration) {
            if (elapsedSwitch >= switchDuration) {
                for (int i=0; i< spriteRenderers.Length; i++) {
                    spriteRenderers[i].enabled = !spriteRenderers[i].enabled;
                }
                elapsedSwitch = 0;
            }
            elapsedSwitch += Time.fixedDeltaTime;

            elapsedTime += Time.fixedDeltaTime;
            yield return null;
        }
        for (int i=0; i< spriteRenderers.Length; i++) {
            spriteRenderers[i].enabled = true;
        }
        if (cb != null) {
            cb();
        }
    }
}

using UnityEngine;
using System.Collections;
using Utils;

public class FlashUsingMaterial : MonoBehaviour {

    public SpriteRenderer[] spriteRenderers;
    private Material baseMaterial; // Assuming all sprite renderers use the same Material
    public Material material;
    public float flashDuration = 1/24f;

	void Start () {
        baseMaterial = spriteRenderers[0].material;
	}

    public void FlashSlugStyle(RetVoidTakeVoid cb = null) {
        if (cb == null) {
            cb = () => { };
        }
        StartCoroutine("FlashCoroutine", cb);
    }

    public void FlashForXSecs(float duration) {
        StartCoroutine("FlashForXSecsCoroutine", duration);
    }

    public void FlashForOneFrame(RetVoidTakeVoid cb = null) {
        if (cb == null) {
            cb = () => { };
        }
        StartCoroutine("FlashForOneFrameCoroutine", cb);
    }


    private IEnumerator FlashForXSecsCoroutine(float u) {
        float elapsed = 0;
        while (elapsed < u) {
            for (int i = 0; i < spriteRenderers.Length; i++) {
                spriteRenderers[i].material = material;
            }
            yield return new WaitForSeconds(flashDuration);        
            elapsed += flashDuration;

            for (int i = 0; i < spriteRenderers.Length; i++) {
                spriteRenderers[i].material = baseMaterial;
            }
            yield return new WaitForSeconds(flashDuration);
            elapsed += flashDuration;
            elapsed += flashDuration;
        }
    }

    private IEnumerator FlashForOneFrameCoroutine(RetVoidTakeVoid cb = null) {
        for (int i = 0; i < spriteRenderers.Length; i++) {
            spriteRenderers[i].material = material;
        }
        yield return new WaitForSeconds(Time.deltaTime);

        for (int i = 0; i < spriteRenderers.Length; i++) {
            spriteRenderers[i].material = baseMaterial;
        }
        if (cb != null) {
            cb();
        }
    }

    private IEnumerator FlashCoroutine(RetVoidTakeVoid cb = null) {
        int numberOfFlashes = 3;
        int flashCount = 0;
        while (flashCount < numberOfFlashes) {
            for (int i = 0; i < spriteRenderers.Length; i++) {
                spriteRenderers[i].material = material;
            }
            yield return new WaitForSeconds(flashDuration);

            for (int i = 0; i < spriteRenderers.Length; i++) {
                spriteRenderers[i].material = baseMaterial;
            }

            yield return new WaitForSeconds(flashDuration);
            flashCount++;
        }
        if (cb != null) {
            cb();
        }
    }
    

}


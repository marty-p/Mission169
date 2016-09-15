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
        // TODO ...
        if (cb == null) {
            cb = () => { };
        }
        StartCoroutine("FlashCoroutine", cb);
    }

    public void FlashForXSecs(float duration) {
        StartCoroutine("FlashForXSecsCoroutine", duration);
    }

    private IEnumerator FlashForXSecsCoroutine(float duration) {
        float elapsed = 0;
        while (elapsed < duration) {
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


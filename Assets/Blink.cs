using UnityEngine;
using System.Collections;

public class Blink : MonoBehaviour {

    private const float blinkDuration = 1.5f;
    private const float switchDuration = 0.04f;
    private float elapsedTime = 0;
    private float elapsedSwitch = 0;
    private SpriteRenderer spriteRenderer;

	// Use this for initialization
	void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
	}
	
    public void BlinkPlease() {
        elapsedTime = 0;
        elapsedSwitch = 0;
        StartCoroutine("BlinkCoroutine");
    }

    private IEnumerator BlinkCoroutine() {
        while (elapsedTime < blinkDuration) {
            if (elapsedSwitch >= switchDuration) {
                spriteRenderer.enabled = !spriteRenderer.enabled;
                elapsedSwitch = 0;
            }
            elapsedSwitch += Time.fixedDeltaTime;

            elapsedTime += Time.fixedDeltaTime;
            yield return null;
        }
        spriteRenderer.enabled = false;
    }
}

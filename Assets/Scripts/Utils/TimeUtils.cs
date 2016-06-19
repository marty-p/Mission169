using System.Collections;
using UnityEngine;
using Utils;

public class TimeUtils : MonoBehaviour {

	public void Delay(float delay, RetVoidTakeVoid cb) {
        StartCoroutine(DelayCoroutine(delay, cb));
    }

    private IEnumerator DelayCoroutine(float delay, RetVoidTakeVoid cb) {
        yield return new WaitForSeconds(delay);
        cb();
    }

}

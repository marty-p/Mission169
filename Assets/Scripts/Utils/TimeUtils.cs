using System.Collections;
using UnityEngine;
using Utils;

public class TimeUtils : MonoBehaviour {

    Coroutine coroutine;
    Coroutine coroutine2;

    public void FrameDelay(RetVoidTakeVoid cb) {
        StartCoroutine(DelayByFrameCoroutine(cb));
    }

	public void TimeDelay(float delay, RetVoidTakeVoid cb) {
        if (coroutine != null) {
            StopCoroutine(coroutine);
        }
        coroutine = StartCoroutine(DelayCoroutine(delay, cb));
    }

    public void RepeatEvery(float period, RetBoolTakeVoid cb) {
        if (coroutine2!= null) {
            StopCoroutine(coroutine2);
        }
       coroutine2 = StartCoroutine(RepeatEveryCoroutine(period, cb));
    }

    private IEnumerator DelayCoroutine(float delay, RetVoidTakeVoid cb) {
        yield return new WaitForSeconds(delay);
        cb();
    }

    private IEnumerator RepeatEveryCoroutine(float period, RetBoolTakeVoid cb) {
        while (cb()) {
            yield return new WaitForSeconds(period);
        }
    }

    private IEnumerator DelayByFrameCoroutine(RetVoidTakeVoid cb) {
        yield return new WaitForFixedUpdate();
        cb();
    }

}

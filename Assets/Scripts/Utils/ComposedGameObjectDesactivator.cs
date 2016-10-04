using System.Collections;
using UnityEngine;

public class ComposedGameObjectDesactivator : MonoBehaviour {

    public GameObject mainObj;
    public GameObject[] objs;

    void OnEnable() {
        mainObj.SetActive(true);
        StartCoroutine("PeriodicCheck");
    }

    private IEnumerator PeriodicCheck() {
        while (true) {
            bool foundActiveObj = false;
            for (int i = 0; i < objs.Length; i++) {
                if (objs[i].activeSelf) {
                    foundActiveObj = true;
                    break;
                }
            }

            if (!foundActiveObj && !mainObj.activeSelf) {
                gameObject.SetActive(false);
            }

            yield return new WaitForSeconds(1);
        }
    }
}

using System.Collections;
using UnityEngine;

public class ComposedGameObjectDesactivator : MonoBehaviour {

    public GameObject[] objs;

    void OnEnable() {
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

            if (!foundActiveObj) {
                gameObject.SetActive(false);
            }

            yield return new WaitForSeconds(1);
        }
    }
}

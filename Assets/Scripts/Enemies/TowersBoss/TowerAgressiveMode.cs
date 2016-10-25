using UnityEngine;
using System.Collections;

public class TowerAgressiveMode : MonoBehaviour {

    public float secBeforeAgressiveFire = 0.7f;
    private Coroutine agressiveFireCoroutine;
    private BoxCollider2D boxCollider;
    public Tower tower;

    void Awake() {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    public void EnableAgressiveMode(bool enabled) {
        boxCollider.enabled = enabled;
    }

    public void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player") {
            agressiveFireCoroutine = StartCoroutine("AgressiveFire");
        }
    }

    public void OnTriggerExit2D(Collider2D collision) {
        if (agressiveFireCoroutine != null  && collision.tag == "Player") {
            StopCoroutine(agressiveFireCoroutine);
        }
    }

    private IEnumerator AgressiveFire() {
        yield return new WaitForSeconds(secBeforeAgressiveFire);
        //tower.Fire(2);
        tower.Fire(1);
    }

}

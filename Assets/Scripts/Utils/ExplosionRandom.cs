using UnityEngine;
using System.Collections;

public class ExplosionRandom : MonoBehaviour {

    private Animator anim;
    private float exploType;

    void Awake() {
        anim = GetComponent<Animator>();

        exploType  = UnityEngine.Random.Range(0f, 1f);
    }

    public void OnEnable() {
        anim.SetTrigger("explode");
        anim.SetFloat("explosion_type", exploType);
    }
}

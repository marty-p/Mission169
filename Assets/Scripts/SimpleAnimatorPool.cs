using UnityEngine;
using System.Collections;

public class SimpleAnimatorPool : MonoBehaviour {

    private static ObjectPoolScript objectPool;

	void Awake () {
        objectPool = GetComponent<ObjectPoolScript>();
	}
	
    public static GameObject GetPooledObject() {
        return objectPool.GetPooledObject();
    }

    public static Animator GetPooledAnimator (RuntimeAnimatorController animatorController = null) {
        GameObject gameObject = GetPooledObject();
        Animator animator = gameObject.GetComponent<Animator>();
        animator.runtimeAnimatorController = animatorController;
        return animator;
    }

}

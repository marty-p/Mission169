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

    public static Animator GetPooledAnimator () {
        GameObject gameObject = GetPooledObject();
        return gameObject.GetComponent<Animator>();
    }

}

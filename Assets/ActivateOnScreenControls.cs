using UnityEngine;
using System.Collections;

public class ActivateOnScreenControls : MonoBehaviour {


    void OnAwake()
    {

#if UNITY_ANDROID
        gameObject.SetActive(true);
#else
        gameObject.SetActive(false);    

#endif
    }

}

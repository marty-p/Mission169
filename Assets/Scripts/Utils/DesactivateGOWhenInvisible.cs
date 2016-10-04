using UnityEngine;
using System.Collections;

namespace Slug {
    public class DesactivateGOWhenInvisible : MonoBehaviour {

        public GameObject go;

        void OnBecameInvisible() {
            go.SetActive(false);
        }

    }
}

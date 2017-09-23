using UnityEngine;

namespace SlugLib
{
    [RequireComponent(typeof(MeshRenderer))]
    public class UvScroll : MonoBehaviour
    {

        [SerializeField] float scrollingFactor = 0.01f;

        private MeshRenderer meshRenderer;
        private Vector2 offset;

        void Start()
        {
            meshRenderer = GetComponent<MeshRenderer>();
        }

        void LateUpdate()
        {
            offset.x += scrollingFactor * Time.deltaTime;
            meshRenderer.material.SetTextureOffset("_MainTex", offset);
        }
    }
}
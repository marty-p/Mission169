using UnityEngine;

namespace SlugLib
{
    public class FollowTarget : MonoBehaviour
    {
        private SpriteRenderer pointOfScrolling;
        public Transform target;
        public bool followActive = true;
        private new Camera camera;
        private Vector2 targetViewPortPos;

        private Vector2 oldTargetPosition;


        void Start()
        {
            camera = GetComponent<Camera>();
        }

        public void InitTarget(Transform target)
        {
            if (target != null)
            {
                this.target = target;
                targetViewPortPos = new Vector2();
                oldTargetPosition = new Vector2(target.transform.position.x, target.transform.position.y);
            }
            else
            {
                Debug.LogError("camera target can't be null");
            }
        }

        void LateUpdate()
        {
            if (target == null)
            {
                return;
            }

            targetViewPortPos = camera.WorldToViewportPoint(target.position);
            if (followActive)
            {
                Follow();
            }
            PreventTargetOutOfCamBounds();
        }

        void Follow()
        {
            if (targetViewPortPos.x > 0.5F)
            {
                Vector3 dest = new Vector3(target.transform.position.x + 0.25f, transform.position.y, transform.position.z);

                Vector3 oldpos = transform.position;
                transform.position = Vector3.Lerp(transform.position, dest, 3f * Time.deltaTime);
            }
        }

        void PreventTargetOutOfCamBounds()
        {
            float dx = oldTargetPosition.x - target.transform.position.x;
            // We still want to be able to move the target manually without the cam preventing it to go out of bounds
            // hence the dx check
            if ((targetViewPortPos.x < 0.03f || targetViewPortPos.x > 1 - 0.03f) && Mathf.Abs(dx) < 1)
            {
                target.transform.position = new Vector2(oldTargetPosition.x, target.transform.position.y);
            }

            oldTargetPosition = target.transform.position;
        }
    }
}

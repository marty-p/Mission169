using UnityEngine;

namespace SlugLib
{
    public static class CameraUtils
    {
        public static Vector2 CamWorldSize;
        public static Transform CamTransform;

        private static Rect bounds;

        public static bool dirty = true;

        public static float GetRightEdgeWorldPosition()
        {
            if (dirty)
            {
                Init();
            };

            return CamTransform.position.x + CamWorldSize.x / 2f;
        }

        public static void DrawCameraEdgesAt(Vector3 center)
        {
            if (dirty)
            {
                Init();
            }

            float xMin = center.x - CamWorldSize.x / 2f;
            float xMax = center.x + CamWorldSize.x / 2f;
            float yMin = center.y - CamWorldSize.y / 2f;
            float yMax = center.y + CamWorldSize.y / 2f;

            Vector2 topLeft = new Vector2(xMin, yMax);
            Vector2 topRight = new Vector2(xMax, yMax);
            Vector2 bottomLeft = new Vector2(xMin, yMin);
            Vector2 bottomRight = new Vector2(xMax, yMin);

            Debug.DrawLine(topLeft, topRight, Color.blue);
            Debug.DrawLine(topRight, bottomRight, Color.blue);
            Debug.DrawLine(bottomRight, bottomLeft, Color.blue);
            Debug.DrawLine(bottomLeft, topLeft, Color.blue);
        }

        public static void EnableFollow(bool enabled)
        {
            FollowTarget follow = Camera.main.GetComponent<FollowTarget>();
            if (follow != null)
            {
                follow.followActive = enabled;
            }
        }

        private static void Init()
        {
            Camera cam = Camera.main;
            CamWorldSize.y = cam.orthographicSize * 2f;
            CamWorldSize.x = CamWorldSize.y * cam.aspect;

            CamTransform = cam.transform;

            //dirty = false;
        }


    }
}

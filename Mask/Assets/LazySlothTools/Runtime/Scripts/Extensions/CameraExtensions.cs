namespace LazySloth.Utilities
{
    using UnityEngine;

    public static class CameraExtensions
    {
        public static bool IsWorldPositionInViewport(this Camera camera, Vector2 worldPosition, float tolerance = 0)
        {
            var screenPos = camera.WorldToViewportPoint(worldPosition);
            return camera.IsScreenPositionInViewport(screenPos, tolerance);
        }

        public static bool IsWorldPositionBelowViewport(this Camera camera, Vector2 worldPosition)
        {
            var screenPos = camera.WorldToViewportPoint(worldPosition);
            return camera.IsScreenPositionBelowViewport(screenPos);
        }
        
        public static bool IsScreenPositionInViewport(this Camera camera, Vector3 screenPosition, float tolerance = 0)
        {
            return screenPosition.x + tolerance >= 0 && screenPosition.x - tolerance <= 1 && screenPosition.y + tolerance >= 0 && screenPosition.y - tolerance <= 1;
        }

        public static bool IsScreenPositionBelowViewport(this Camera camera, Vector3 screenPosition)
        {
            return screenPosition.y < 0;
        }
    }
}
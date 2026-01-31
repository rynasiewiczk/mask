namespace LazySloth.Utilities
{
    using UnityEngine;

    public static class RectTransformExtensions
    {
        public static Rect GetWorldRect(this RectTransform rTransform)
        {
            Vector3[] corners = new Vector3[4];
            rTransform.GetWorldCorners(corners);

            var sizeX = Vector2.Distance(corners[1], corners[2]);
            var sizeY = Vector2.Distance(corners[0], corners[1]);

            return new Rect(corners[0], new Vector2(sizeX, sizeY));
        }

        public static float GetMaxWorldSize(this RectTransform rTransform)
        {
            var worldRect = GetWorldRect(rTransform);
            return Mathf.Max(worldRect.height, worldRect.width) / 2f;
        }
    }
}
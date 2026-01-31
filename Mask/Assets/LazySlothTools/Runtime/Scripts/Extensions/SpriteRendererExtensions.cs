namespace LazySloth.Utilities
{
    using UnityEngine;

    public static class SpriteRendererExtensions
    {
        public static void SetColorAlpha(this SpriteRenderer renderer, float alpha)
        {
            var rendererColor = renderer.color;
            rendererColor.a = alpha;
            renderer.color = rendererColor;
        }
    }
}
namespace LazySloth.Utilities
{
    using UnityEngine;

    public static class ColorExtensions
    {
        public static Color orange => new Color(1f, .5f, 0f, 1);

        public static Color SetAlpha(this Color c, float alpha)
        {
            return new Color(c.r, c.g, c.b, alpha);
        }

        public static Color SetIntensity(this Color c, float intensity)
        {
            float factor = Mathf.Pow(2, intensity);
            return new Color(c.r * factor, c.g * factor, c.b * factor);
        }
    }
}
namespace LazySloth.Utilities
{
    using UnityEngine;

    public static class TextMeshProExtensions
    {
        public static string SetTMPColor(this string text, Color color)
        {
            var hexColor = ColorUtility.ToHtmlStringRGB(color);
            return $"<#{hexColor}>{text}</color>";
        }
    }
}
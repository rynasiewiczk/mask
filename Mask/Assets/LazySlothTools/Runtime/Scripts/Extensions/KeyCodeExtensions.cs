namespace LazySloth.Utilities
{
    using UnityEngine;

    public static class KeyCodeExtensions
    {
        public static string RemoveAlpha(this KeyCode keyCode)
        {
            return keyCode.ToString().Replace("Alpha", "");
        }
    }
}
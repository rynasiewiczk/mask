namespace LazySloth.Utilities
{
    using System.Collections.Generic;
    using UnityEngine;

    public static class ArrayExtensions
    {
        public static T GetRandom<T>(this T[] items)
        {
            return items[Random.Range(0, items.Length)];
        }

        public static T GetRandom<T>(this IReadOnlyList<T> items)
        {
            return items[Random.Range(0, items.Count)];
        }
    }
}
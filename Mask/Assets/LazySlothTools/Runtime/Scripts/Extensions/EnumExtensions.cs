namespace LazySloth.Utilities
{
    using System;
    using System.Linq;
    using UnityEngine;

    public static class EnumExtensions
    {
        public static T Next<T>(T src, bool loop = true) where T : struct
        {
            if (!typeof(T).IsEnum) { throw new ArgumentException(String.Format("Argument {0} is not an Enum", typeof(T).FullName)); }

            var array = (T[]) Enum.GetValues(src.GetType());
            var j = Array.IndexOf(array, src) + 1;
            if (loop)
            {
                return (array.Length == j) ? array.First() : array[j];
            }
            else
            {
                return array[Mathf.Clamp(j, 0, array.Length - 1)];
            }
        }
        
        public static T Previous<T>(T src, bool loop = true) where T : struct
        {
            if (!typeof(T).IsEnum) { throw new ArgumentException(String.Format("Argument {0} is not an Enum", typeof(T).FullName)); }

            var array = (T[]) Enum.GetValues(src.GetType());
            var j = Array.IndexOf(array, src) - 1;
            if (loop)
            {
                return (j == -1) ? array.Last() : array[j];
            }
            else
            {
                return array[Mathf.Clamp(j, 0, array.Length - 1)];
            }
        }
    }
}
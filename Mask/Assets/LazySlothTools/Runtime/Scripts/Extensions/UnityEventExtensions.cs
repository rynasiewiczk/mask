namespace LazySloth.Utilities
{
    using System;
    using UnityEngine.Events;

    public static class UnityEventExtensions
    {
        public static void AddListener(this UnityEvent unityEvent, Action action)
        {
            unityEvent.AddListener(() => action?.Invoke());
        }
    }
}
namespace LazySloth.Utilities
{
    using UnityEngine;

    public static class TransformExtensions
    {
        public static string GetHierarchyPath(this Transform t)
        {
            string path = "/" + t.name;
            while (t.parent != null)
            {
                t = t.parent;
                path = "/" + t.name + path;
            }
            return path;
        }

        public static T GetComponentComponentInGrands<T>(this Transform t) where T : class
        {
            T component = null;
            Transform currentT = t;
            do
            {
                component = currentT.GetComponent<T>();
                currentT = currentT.parent;
            } while (component == null || currentT != null);

            return component;
        }
    }
}
namespace LazySloth.Utilities
{
    using UnityEngine;
    
    public static class Vector3Extensions
    {
        public static Vector3 AddToX(this Vector3 v3, float value) => new Vector3(v3.x + value, v3.y, v3.z);
        public static Vector3 AddToY(this Vector3 v3, float value) => new Vector3(v3.x, v3.y + value, v3.z);
        public static Vector3 AddToZ(this Vector3 v3, float value) => new Vector3(v3.x, v3.y, v3.z + value);

        public static Vector3 SetX(this Vector3 v3, float value) => new Vector3(value, v3.y, v3.z); 
        public static Vector3 SetY(this Vector3 v3, float value) => new Vector3(v3.x, value, v3.z); 
        public static Vector3 SetZ(this Vector3 v3, float value) => new Vector3(v3.x, v3.y, value);
        public static Vector3 Mul(this Vector3 v, Vector3 v2) { return new Vector3(v.x * v2.x, v.y * v2.y, v.z * v2.z); }

        public static float GetMax(this Vector3 v3) => Mathf.Max(v3.x, v3.y, v3.z);

        public static Vector2 ToVector2(this Vector3 v3) => v3;

        public static Vector2 ZtoY(this Vector3 v3) => new Vector2(v3.x, v3.z); 
        public static Vector3 DirectionTo(this Vector3 v1, Vector3 v2)
        {
            Vector3 heading = (v1 - v2);
            return heading / heading.magnitude;
        }
        
        public static Vector3 Clamp(this Vector3 v3, Vector3 min, Vector3 max)
        {
            var x = Mathf.Clamp(v3.x, min.x, max.x);
            var y = Mathf.Clamp(v3.y, min.y, max.y);
            var z = Mathf.Clamp(v3.z, min.z, max.z);

            return new Vector3(x, y, z);
        }

        public static Vector3 FlipX(this Vector3 v3)
        {
            return new Vector3(-v3.x, v3.y, v3.z);
        }

        public static Vector3 FlipY(this Vector3 v3)
        {
            return new Vector3(v3.x, -v3.y, v3.z);
        }

        public static Vector3 FlipZ(this Vector3 v3)
        {
            return new Vector3(v3.x, v3.y, -v3.z);
        }
    }
}

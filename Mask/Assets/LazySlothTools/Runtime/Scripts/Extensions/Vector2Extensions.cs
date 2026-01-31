namespace LazySloth.Utilities
{
    using UnityEngine;

    public static class Vector2Extensions
	{
		public static Vector3 YtoZ(this Vector2 v2) { return new Vector3 (v2.x, 0f, v2.y); }

        public static Vector2 Div(this Vector2 v1, Vector2 v2) { return new Vector2(Mathf.Abs(v2.x) > 0.0001 ? v1.x / v2.x : 0f, Mathf.Abs(v2.y) > 0.0001 ? v1.y / v2.y : 0); }

        public static Vector2 SetY(this Vector2 v, float y) { return new Vector2(v.x, y); }
        public static Vector2 SetX(this Vector2 v, float x) { return new Vector2(x, v.y); }

        public static Vector2 Sub(this Vector2 v, float value) { return new Vector2(v.x - value, v.y - value); }
        public static Vector2 Add(this Vector2 v, float value) { return new Vector2(v.x + value, v.y + value); }
        public static Vector2 Mul(this Vector2 v, Vector2 v2) { return new Vector2(v.x * v2.x, v.y * v2.y); }

        public static Vector2 Direction(Vector2 from, Vector2 to)
        {
            var heading = from - to;
            return heading / heading.magnitude;
        }

        public static Vector2 Rotate(this Vector2 v, float degrees)
        {
            float sin = Mathf.Sin(-degrees * Mathf.Deg2Rad);
            float cos = Mathf.Cos(-degrees * Mathf.Deg2Rad);

            float tx = v.x;
            float ty = v.y;
            v.x = (cos * tx) - (sin * ty);
            v.y = (sin * tx) + (cos * ty);
            return v;
        }

        public static Vector3 ToVector3(this Vector2 v2)
        {
            return new Vector3(v2.x, v2.y);
        }
        
        public static Vector2 GetClosestPoint(Vector2 origin, params Vector2[] points)
        {
            var closest = origin;
            var smallestDistance = float.MaxValue;

            foreach (var vector in points)
            {
                var distance = Vector2.Distance(origin, vector);
                if (distance < smallestDistance)
                {
                    smallestDistance = distance;
                    closest = vector;
                }
            }

            return closest;
        }

        public static Vector2 RandomDirection()
        {
            var x = Random.Range(-1f, 1f);
            var y = Random.Range(-1f, 1f);

            var vector = new Vector2(x,y).normalized;
            return vector;
        }

        public static Vector2 Clamp(this Vector2 v2, Rect rect)
        {
            v2.x = Mathf.Clamp(v2.x, rect.xMin, rect.xMax);
            v2.y = Mathf.Clamp(v2.y, rect.yMin, rect.yMax);

            return v2;
        }
        
        public static Vector2 Clamp(this Vector2 v2, Vector2 min, Vector2 max)
        {
            var x = Mathf.Clamp(v2.x, min.x, max.x);
            var y = Mathf.Clamp(v2.y, min.y, max.y);

            return new Vector2(x, y);
        }
    }
}

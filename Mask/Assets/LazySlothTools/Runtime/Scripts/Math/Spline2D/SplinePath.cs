namespace LazySloth.Utilities.Math
{
    using System.Collections.Generic;
    using UnityEngine;

    public sealed class SplinePath
    {
        private readonly Spline2D _spline2D;

        public float Length { get; }

        public SplinePath(List<Vector2> points)
        {
            _spline2D = new Spline2D(points);
            Length = _spline2D.Length;
        }

        public Vector2 GetPositionOnPath(float normalizedDistance)
        {
            var distance = Length * normalizedDistance;
            return _spline2D.InterpolateDistance(distance);
        }
    }
}
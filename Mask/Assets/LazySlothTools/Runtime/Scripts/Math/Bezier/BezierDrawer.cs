namespace LazySloth.Utilities.Math
{
    using UnityEngine;

    public class BezierDrawer : MonoBehaviour
    {
        [SerializeField] private Color _mainColor = Color.clear;
        [SerializeField] private Color _curveColor = Color.clear;
        [SerializeField] private Color _pointsColor = Color.blue;

        [SerializeField] private float _sphereRadius = .5f;

        [Space, SerializeField] public Vector2 _startPoint = Vector2.zero;
        [SerializeField] public Vector2 _endPoint = Vector2.zero;
        [SerializeField] public Vector2 _controlPoint = Vector2.zero;

        [SerializeField] [Range(0, 1f)] private float _interpolation = 0f;

        [SerializeField] private int _steps = 100;

        private void OnDrawGizmos()
        {
            if (_steps <= 0)
            {
                return;
            }

            GUI.color = _mainColor;
            Gizmos.DrawLine(_startPoint, _endPoint);

            var currentPoint = Bezier.GetPosition(_startPoint, _controlPoint, _endPoint, _interpolation);
            Gizmos.DrawSphere(currentPoint, _sphereRadius);

            Gizmos.color = _curveColor;
            for (int i = 0; i < _steps; i++)
            {
                var interpolation = 1f / _steps * i;
                var point = Bezier.GetPosition(_startPoint, _controlPoint, _endPoint, interpolation);

                var nextInterpolation = 1f / _steps * (i + 1);
                nextInterpolation = Mathf.Min(nextInterpolation, 1);
                var nextPoint = Bezier.GetPosition(_startPoint, _controlPoint, _endPoint, nextInterpolation);
                Gizmos.DrawLine(point, nextPoint);
            }

            Gizmos.color = _pointsColor;
            Gizmos.DrawSphere(_startPoint, _sphereRadius);
            Gizmos.DrawSphere(_controlPoint, _sphereRadius);
            Gizmos.DrawSphere(_endPoint, _sphereRadius);
        }
    }
}
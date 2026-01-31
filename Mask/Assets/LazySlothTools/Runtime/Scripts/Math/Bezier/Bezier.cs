namespace LazySloth.Utilities.Math
{
    using UnityEngine;
    
    public class Bezier
    {
        public Vector3 StartPosition { get; }
        public Vector3 EndPosition { get; }
        public Vector3 MidPosition { get; }

        private readonly int _approximationResolution;
        private bool _lengthWasCalculated;
        private float _approximatedLength;

        public float ApproximatedLength
        {
            get
            {
                if (!_lengthWasCalculated)
                {
                    Debug.LogWarning($"Length wasn't calculated! Calculated now!");
                    _approximatedLength = CalculateLength();
                }
                return _approximatedLength;
            }
        }
        

        public Bezier(Vector3 startPosition, Vector3 midPosition, Vector3 endPosition, bool calculateLength = false, int approximationResolution = 20)
        {
            StartPosition = startPosition;
            EndPosition = endPosition;
            MidPosition = midPosition;

            _approximationResolution = approximationResolution;
            
            if (calculateLength)
            {
                _approximatedLength = CalculateLength();
            }
        }

        private float CalculateLength()
        {
            var resolution = _approximationResolution;
            var delta = 1f / resolution;

            var length = 0f;
            var p1 = StartPosition;
            for (var i = 1; i < resolution; i++)
            {
                var p2 = GetPosition(delta * i);
                length += Vector3.Distance(p1, p2);
                p1 = p2;
            }

            _lengthWasCalculated = true;
            return length;
        }
        
        public Vector3 GetPosition(float t)
        {
            return GetPosition(StartPosition, MidPosition, EndPosition, t);
        }
        
        public static Vector3 GetPosition(Vector3 p0, Vector3 p1, Vector3 p2, float t)
        {
            var pFinalX = Mathf.Pow(1 - t, 2) * p0.x +
                          (1 - t) * 2 * t * p1.x +
                          t * t * p2.x;
            
            var pFinalY = Mathf.Pow(1 - t, 2) * p0.y +
                          (1 - t) * 2 * t * p1.y +
                          t * t * p2.y;
            
            var pFinalZ = Mathf.Pow(1 - t, 2) * p0.z +
                          (1 - 1) * 2 * t * p1.z +
                          t * t * p2.z;
            
            var pFinal = new Vector3(pFinalX, pFinalY, pFinalZ);

            return pFinal;
        }
    }
}
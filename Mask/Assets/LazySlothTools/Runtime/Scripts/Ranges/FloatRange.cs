namespace LazySloth.Utilities
{
    using System;
    using UnityEngine;

    [Serializable]
    public class FloatRange
    {
        [SerializeField] private float _min;
        [SerializeField] private float _max;
        
        public float Max => _max;

        public float GetRandom()
        {
            return UnityEngine.Random.Range(_min, _max);
        }

        public float Evaluate(float t)
        {
            return Mathf.Lerp(_min, _max, t);
        }
    }
    
}
namespace LazySloth.Utilities
{
    using System;
    using UnityEngine;

    [Serializable]
    public class FloatRange : Range<float>
    {
        public FloatRange() : base() {  } 
        public FloatRange(float min, float max) : base(min, max) {  }

        public float GetRandom()
        {
            return UnityEngine.Random.Range(Minimum, Maximum);
        }

        public float Evaluate(float t)
        {
            return Mathf.Lerp(Minimum, Maximum, t);
        }
    }
    
}
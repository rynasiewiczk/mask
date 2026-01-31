namespace LazySloth.Utilities
{
    using System;

    [Serializable]
    public class IntRange : Range<int>
    {
        public IntRange() : base() {  } 
        public IntRange(int min, int max) : base(min, max) {  }
        
        public int GetRandom()
        {
            return UnityEngine.Random.Range(Minimum, Maximum);
        }
    }
}
namespace LazySloth.Utilities
{
    using UnityEngine;

    public static class RandomExtension
    {
        public static int RandomSign()
        {
            return Random.value > .5f ? 1 : -1;
        }

        public static bool RandomBool()
        {
            return Random.value > .5f;
        }

        public static bool RandomProbability(int range, int whenSuccessRange)
        {
            return Random.Range(0, range) <= whenSuccessRange;
        }
    }
}
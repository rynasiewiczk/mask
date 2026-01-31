namespace LazySloth.Utilities
{
    using UnityEngine;
    
    public static class FloatExtensions
    {
        public static bool Approximately(this float f, float equalTo)
        {
            return Mathf.Approximately(f, equalTo);
        }

        public static bool ApproximatelyZero(this float f)
        {
            return f.Approximately(0f);
        }

        public static float ToRad(this float f)
        {
            return f * Mathf.Deg2Rad;
        }

        public static float ToDeg(this float f)
        {
            return f * Mathf.Rad2Deg;
        }

        //TODO: name it better
        //this will be give wrong result for angle that is bigger than 2PI
        public static float RoundedToDegrees(this float f)
        {
            return (f + (Mathf.PI * 2)) % (Mathf.PI * 2);
        }

        /// <summary>
        /// Normalize angle to 0 - 2Pi
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public static float ToFullRadian(this float f)
        {
            const float twoPi = Mathf.PI * 2f;
            var a = f % twoPi;
            if (a < 0f)
            {
                a += twoPi;
            }

            return a;
        }
        
        /// <summary>
        /// Normalize angle to 0 - 360
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public static float ToFullDegree(this float f)
        {
            return f.ToRad().ToFullRadian().ToDeg();
        }
        
        public static float Map(this float value, float fromBottom, float toBottom, float fromTop, float toTop)
        {
            if (value < fromBottom) return toBottom;
            if (value > fromTop) return toTop;
            
            return (value - fromBottom) / (fromTop - fromBottom) * (toTop - toBottom) + toBottom;
        }
    }
}
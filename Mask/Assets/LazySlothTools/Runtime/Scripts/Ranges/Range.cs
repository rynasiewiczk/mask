namespace LazySloth.Utilities
{
    using UnityEngine;
    using System;
    
    [Serializable]
    public class Range<T> where T : IComparable<T>
    {
        [SerializeField, HideInInspector]   
        private T _min;
        [SerializeField, HideInInspector]
        private T _max;
        /// <summary>Minimum value of the range.</summary>
        
        public virtual T Minimum
        {
            get => _min; 
            set
            {
                _min = value;
                //TODO: remap min max value
                //_min = Maximum.CompareTo(value) > 0 ? _max : value;
                //Maximum = Maximum.CompareTo(value) > 0 ? value : Maximum;
                
            }
        }

        /// <summary>Maximum value of the range.</summary>
        public virtual T Maximum
        {
            get => _max;
            set
            {
                _max = value;
                //TODO: remap min max value
                //_max = Minimum.CompareTo(value) > 0 ? _min : value;
                //Minimum = Minimum.CompareTo(value) > 0 ? value : Minimum;
            }
        }

        /// <summary>
        /// To create type in inspector. Do not use to create from code!!
        /// </summary>
        public Range()
        {
        }

        public Range(T min, T max)
        {
            Minimum = min;
            Maximum = max;
        }

        /// <summary>Presents the Range in readable format.</summary>
        /// <returns>String representation of the Range</returns>
        public override string ToString()
        {
            return $"[{Minimum} - {Maximum}]";
        }

        /// <summary>Determines if the range is valid.  </summary>
        /// <returns>True if range is valid, else false</returns>
        public bool IsValid()
        {
            return Minimum.CompareTo(Maximum) <= 0;
        }

        /// <summary>Determines if the provided value is inside the range.</summary>
        /// <param name="value">The value to test</param>
        /// <returns>True if the value is inside Range, else false</returns>
        public bool ContainsValue(T value)
        {
            return (this.Minimum.CompareTo(value) <= 0) && (value.CompareTo(this.Maximum) <= 0);
        }

        /// <summary>Determines if this Range is inside the bounds of another range.</summary>
        /// <param name="Range">The parent range to test on</param>
        /// <returns>True if range is inclusive, else false</returns>
        public bool IsInsideRange(Range<T> range)
        {
            return this.IsValid() && range.IsValid() && range.ContainsValue(this.Minimum) && range.ContainsValue(this.Maximum);
        }

        /// <summary>Determines if another range is inside the bounds of this range.</summary>
        /// <param name="Range">The child range to test</param>
        /// <returns>True if range is inside, else false</returns>
        public bool ContainsRange(Range<T> range)
        {
            return this.IsValid() && range.IsValid() && this.ContainsValue(range.Minimum) && this.ContainsValue(range.Maximum);
        }

        public T Clamp(T value)
        {
            return Minimum.CompareTo(value) >= 0 ? Minimum : (value.CompareTo(this.Maximum) <= 0 ? value : Maximum);
        }
    }
}

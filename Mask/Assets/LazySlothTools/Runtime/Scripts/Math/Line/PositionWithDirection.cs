namespace LazySloth.Utilities.Math
{
    using UnityEngine;

    public struct PositionWithDirection
    {
        public Vector2 Start;
        public Vector2 Direction;
        
        public PositionWithDirection(Vector2 start, Vector2 direction)
        {
            Start = start;
            Direction = direction;
        }
    }
}
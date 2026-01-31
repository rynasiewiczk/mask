namespace LazySloth.Pong.Gameplay
{
    using UnityEngine;

    public struct Trigger2DInfo
    {
        public Collider2D Other;
        public CollisionTag Tag;

        public Trigger2DInfo(Collider2D other, CollisionTag tag)
        {
            Other = other;
            Tag = tag;
        }
    }
}
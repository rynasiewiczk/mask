namespace LazySloth.Pong.Gameplay
{
    using System;

    public interface ITriggerCollision2DProxy
    {
        event Action<Trigger2DInfo> OnTriggerEnter2D;
        event Action<Trigger2DInfo> OnTriggerStay2D;
        event Action<Trigger2DInfo> OnTriggerExit2D;
    }
}
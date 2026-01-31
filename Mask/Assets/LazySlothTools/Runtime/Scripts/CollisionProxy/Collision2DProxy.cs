namespace LazySloth.Pong.Gameplay
{
    using System;
    using UnityEngine;

    public class Collision2DProxy : MonoBehaviour, ITriggerCollision2DProxy
    {
        public event Action<Trigger2DInfo> OnTriggerEnter2D;
        public event Action<Trigger2DInfo> OnTriggerStay2D;
        public event Action<Trigger2DInfo> OnTriggerExit2D;

        [SerializeField] private Collider2D _collider = default;
        
        public Collider2D Collider => _collider;

        public void TriggerEnter2D(Trigger2DInfo triggerData)
        {
            OnTriggerEnter2D?.Invoke(triggerData);
        }

        public void TriggerExit2D(Trigger2DInfo triggerData)
        {
            OnTriggerExit2D?.Invoke(triggerData);
        }

        public void TriggerStay2D(Trigger2DInfo triggerData)
        {
            OnTriggerStay2D?.Invoke(triggerData);
        }
    }
}